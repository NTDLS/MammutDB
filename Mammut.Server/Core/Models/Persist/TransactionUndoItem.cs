using System;
using System.IO;
using static Mammut.Server.Core.Constants;

namespace Mammut.Server.Core.Models.Persist
{
    /// <summary>
    /// Represents a single transaction item. This item will define how to undo any action which was taken by the IO engine.
    /// </summary>
    public class TransactionUndoItem
    {
        public Guid Id { get; set; }

        public TransactionUndoAction UndoAction { get; set; }

        /// <summary>
        /// The original path of the file/directory which was modified.
        /// </summary>
        public string OriginalPath { get; set; }

        /// <summary>
        /// The path of any backup which was taken before a file/directory was modified.
        /// </summary>
        public string BackupPath { get; set; }

        public void Execute()
        {
            if (UndoAction == TransactionUndoAction.DeleteDirectory)
            {
                if (Directory.Exists(OriginalPath))
                {
                    Directory.Delete(OriginalPath, true);
                }
            }
            else if (UndoAction == TransactionUndoAction.RestoreDirectory)
            {
                if (Directory.Exists(OriginalPath))
                {
                    Directory.Delete(OriginalPath, true);
                }
                Directory.Move(BackupPath, OriginalPath);
            }
            else if (UndoAction == TransactionUndoAction.DeleteFile)
            {
                if (File.Exists(OriginalPath))
                {
                    File.Delete(OriginalPath);
                }
            }
            else if (UndoAction == TransactionUndoAction.RestoreFile)
            {
                if (File.Exists(OriginalPath))
                {
                    File.Delete(OriginalPath);
                }

                File.Move(BackupPath, OriginalPath);
            }
            else
            {
                throw new Exception("Transaction undo type not implemented.");
            }
        }
    }
}
