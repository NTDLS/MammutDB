using System;
using System.Collections.Generic;

namespace Mammut.Server.Core.Models.Persist
{
    /// <summary>
    /// Represents a collection of transactions. (undo items)
    /// </summary>
    [Serializable]
    public class MetaTransactionUndoItemCollection
    {
        public List<MetaTransactionUndoItem> Catalog = new List<MetaTransactionUndoItem>();

        public void Add(MetaTransactionUndoItem meta)
        {
            Catalog.Add(meta);
        }
    }
}
