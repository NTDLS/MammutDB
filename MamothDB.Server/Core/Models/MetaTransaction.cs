using MamothDB.Server.Core.Models.Persist;
using System;
using System.Collections.Generic;
using System.IO;
using static MamothDB.Server.Core.Constants;

namespace MamothDB.Server.Core.Models
{
    public class MetaTransaction
    {
        private MetaSession _session;
        private ServerCore _core;
        private MetaLatchKeyCollection _latchKeys = new MetaLatchKeyCollection();
        private HashSet<string> transactedItems = new HashSet<string>();
        private int _references = 0;

        /// <summary>
        /// Implicit transactions were created by the engine and must be completed at the end of any action.
        /// </summary>
        public bool IsImplicit { get; private set; }
        public Guid Id { get; private set; }
        public DeferredDiskIO DeferredIO { get; private set; }

        /// <summary>
        /// This collection is ONLY used to serialize to the undo log. It is not for iteration.
        /// </summary>
        private TransactionUndoItemCollection _undoActions = new TransactionUndoItemCollection();

        public MetaTransaction(ServerCore core, Guid transactionId)
        {
            _core = core;
            IsImplicit = false;
            Id = transactionId;
            DeferredIO = new DeferredDiskIO(core);
        }

        public MetaTransaction(ServerCore core, MetaSession session, bool isImplicit)
        {
            _core = core;
            IsImplicit = isImplicit;
            _session = session;
            Id = Guid.NewGuid();
            DeferredIO = new DeferredDiskIO(core);
        }

        public string TransactionBackupPath
        {
            get
            {
                return Path.Combine(_core.Settings.TransactionPath, Id.ToString());
            }
        }

        public string TransactionUndoCatalogFile
        {
            get
            {
                return Path.Combine(_core.Settings.TransactionPath, Id.ToString(), Constants.Filesystem.TransactionUndoCatalog);
            }
        }

        /// <summary>
        /// Adds a key to the transaction for an outstanding latch.
        /// </summary>
        /// <param name="latchKey"></param>
        public void AddLatchKey(MetaLatchKey latchKey)
        {
            _latchKeys.Add(latchKey);
        }

        /// <summary>
        /// Pass-through to the latch engine to AcquireSchemaLatch.
        /// </summary>
        /// <param name="logicalSchemaPath"></param>
        /// <param name="latchMode"></param>
        public void AcquireSchemaLatch(string logicalSchemaPath, LatchMode latchMode)
        {
            _core.Latch.AcquireSchemaLatch(_session, logicalSchemaPath, latchMode);
        }

        /// <summary>
        /// Pass-through to the latch engine to AcquireDocumentLatch.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="logicalDocumentPath"></param>
        /// <param name="latchMode"></param>
        public void AcquireDocumentLatch(MetaSession session, string logicalDocumentPath, LatchMode latchMode)
        {
            _core.Latch.AcquireDocumentLatch(_session, logicalDocumentPath, latchMode);
        }

        public void AddUndoAction(TransactionUndoAction undoAction, string originalPath)
        {
            _undoActions.Add(new TransactionUndoItem
            {
                Id = Guid.NewGuid(),
                OriginalPath = originalPath,
                UndoAction = undoAction
            });
        }

        public void AddUndoAction(TransactionUndoAction undoAction, string originalPath, string backupPath)
        {
            _undoActions.Add(new TransactionUndoItem
            {
                Id = Guid.NewGuid(),
                OriginalPath = originalPath,
                BackupPath = backupPath,
                UndoAction = undoAction
            });
        }

        /// <summary>
        /// Increases the referecne count of the current transaction.
        /// </summary>
        public void Enlist()
        {
            _references++;
        }

        /// <summary>
        /// Decreases the transaction reference count. If it falls to zero, the transaction is comitted.
        /// </summary>
        /// <returns></returns>
        public bool Commit()
        {
            _references--;

            if (_references < 0)
            {
                throw new Exception("Transaction reference count fell below zero.");
            }

            if (_references == 0)
            {
                _latchKeys.TurnInAllKeys();

                if (Directory.Exists(TransactionBackupPath))
                {
                    Directory.Delete(TransactionBackupPath, true);
                }

                DeferredIO.Commit();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Unconditionally rolls back the current transaction.
        /// </summary>
        public void Rollback()
        {
            string txUndoCatalogFile = Path.Combine(_core.Settings.TransactionPath, Id.ToString(), Constants.Filesystem.TransactionUndoCatalog);
            var undoCollection = _core.IO.GetJsonDirty<TransactionUndoItemCollection>(txUndoCatalogFile);
            var undoActions = undoCollection.Catalog;

            undoActions.Reverse();

            foreach (var txUndoAction in undoActions)
            {
                txUndoAction.Execute();
            }

            _latchKeys.TurnInAllKeys();

            if (Directory.Exists(TransactionBackupPath))
            {
                Directory.Delete(TransactionBackupPath, true);
            }
        }

        /// <summary>
        /// Writes the transaction catalog to disk. No caching.
        /// </summary>
        public void CheckpointCatalog()
        {
            if (Directory.Exists(TransactionBackupPath) == false)
            {
                Directory.CreateDirectory(TransactionBackupPath);
            }
            _core.IO.PutJsonDirty(TransactionUndoCatalogFile, _undoActions);
        }

        /// <summary>
        /// Records the action of creating a directory to that it can be undone.
        /// </summary>
        /// <param name="filePath"></param>
        public void RecordCreateDirectory(string filePath)
        {
            var key = filePath.ToLower();
            if (transactedItems.Contains(key) == false)
            {
                transactedItems.Add(key);
                if (File.Exists(filePath) == false)
                {
                    if (Directory.Exists(TransactionBackupPath) == false)
                    {
                        Directory.CreateDirectory(TransactionBackupPath);
                    }
                    AddUndoAction(Constants.TransactionUndoAction.DeleteDirectory, filePath);
                }

                CheckpointCatalog();
            }
        }

        /// <summary>
        /// Records the deletion of a directory so that it can be undone. This method actually moves the folder to the undo location because that is the most efficient way to do it.
        /// </summary>
        /// <param name="filePath"></param>
        public void RecordDeleteDirectory(string filePath)
        {
            var key = filePath.ToLower();
            if (transactedItems.Contains(key) == false)
            {
                transactedItems.Add(key);

                if (File.Exists(filePath) == false)
                {
                    if (Directory.Exists(TransactionBackupPath) == false)
                    {
                        Directory.CreateDirectory(TransactionBackupPath);
                    }

                    string backupFile = Path.Combine(TransactionBackupPath, Guid.NewGuid().ToString());
                    Directory.Move(filePath, backupFile);
                    AddUndoAction(Constants.TransactionUndoAction.RestoreDirectory, filePath, backupFile);
                }

                CheckpointCatalog();
            }
        }

        /// <summary>
        /// Records the action of writing to a file so that it can be undone.
        /// </summary>
        /// <param name="filePath"></param>
        public void RecordFileWrite(string filePath)
        {
            var key = filePath.ToLower();
            if (transactedItems.Contains(key) == false)
            {
                transactedItems.Add(key);

                if (File.Exists(filePath))
                {
                    if (Directory.Exists(TransactionBackupPath) == false)
                    {
                        Directory.CreateDirectory(TransactionBackupPath);
                    }

                    string backupFile = Path.Combine(TransactionBackupPath, Guid.NewGuid().ToString() + Constants.Filesystem.TxUnDoExtension);
                    File.Copy(filePath, backupFile);
                    AddUndoAction(Constants.TransactionUndoAction.RestoreFile, filePath, backupFile);
                }
                else
                {
                    AddUndoAction(Constants.TransactionUndoAction.DeleteFile, filePath);
                }

                CheckpointCatalog();
            }
        }
    }
}
