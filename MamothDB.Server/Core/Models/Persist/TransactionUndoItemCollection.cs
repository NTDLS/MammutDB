﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MamothDB.Server.Core.Models.Persist
{
    /// <summary>
    /// Represents a collection of transactions. (undo items)
    /// </summary>
    [Serializable]
    public class TransactionUndoItemCollection
    {
        public List<TransactionUndoItem> Catalog = new List<TransactionUndoItem>();

        public void Add(TransactionUndoItem meta)
        {
            Catalog.Add(meta);
        }
    }
}
