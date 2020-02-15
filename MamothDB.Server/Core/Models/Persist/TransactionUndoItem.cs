using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MamothDB.Server.Core.Constants;

namespace MamothDB.Server.Core.Models.Persist
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
    }
}
