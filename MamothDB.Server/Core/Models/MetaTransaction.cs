using MamothDB.Server.Core.Models.Persist;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static MamothDB.Server.Core.Constants;

namespace MamothDB.Server.Core.Models
{
    public class MetaTransaction
    {
        private ServerCore _core { get; set; }
        /// <summary>
        /// Implicit transactions were created by the engine and must be completed at the end of any action.
        /// </summary>
        public bool IsImplicit { get; private set; }
        public Guid Id { get; private set; }
        public MetaSession Session { get; private set; }
        public TransactionUndoItemCollection UndoActions { get; private set; }

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

        public void AddUndoAction(TransactionUndoAction undoAction, string originalPath)
        {
            UndoActions.Add(new TransactionUndoItem
            {
                Id = Guid.NewGuid(),
                OriginalPath = originalPath,
                UndoAction = undoAction
            });

        }

        public void AddUndoAction(TransactionUndoAction undoAction, string originalPath, string backupPath)
        {
            UndoActions.Add(new TransactionUndoItem
            {
                Id = Guid.NewGuid(),
                OriginalPath = originalPath,
                BackupPath = backupPath,
                UndoAction = undoAction
            });
        }

        public MetaTransaction(ServerCore core, MetaSession session, bool isImplicit)
        {
            _core = core;
            IsImplicit = isImplicit;
            Session = session;
            UndoActions = new TransactionUndoItemCollection();
            Id = Guid.NewGuid();
        }

        public void Commit()
        {
            Session.CurrentTransaction = null;
        }

        public void Rollback()
        {
            Session.CurrentTransaction = null;
        }

        public void CheckpointCatalog()
        {
            _core.IO.PutJsonDirty(TransactionUndoCatalogFile, UndoActions);
        }

        public void RecordCreateDirectory(string filePath)
        {
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

        public void RecordFileWrite(string filePath)
        {
            if (File.Exists(filePath))
            {
                if (Directory.Exists(TransactionBackupPath) == false)
                {
                    Directory.CreateDirectory(TransactionBackupPath);
                }

                string backupFile = Path.Combine(TransactionBackupPath, Guid.NewGuid().ToString());
                File.Copy(filePath, backupFile);
                AddUndoAction(Constants.TransactionUndoAction.DeleteFile, filePath, backupFile);
            }
            else
            {
                AddUndoAction(Constants.TransactionUndoAction.DeleteFile, filePath);
            }

            CheckpointCatalog();
        }
    }
}
