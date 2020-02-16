using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MamothDB.Server.Core.Models
{
    /// <summary>
    /// Represents a collection of keys in a catalog.
    /// </summary>
    [Serializable]
    public class MetaLatchKeyCollection
    {
        private List<MetaLatchKey> Catalog = new List<MetaLatchKey>();

        public MetaLatchKeyCollection()
        {
        }

        public void Add(MetaLatchKey key)
        {
            Catalog.Add(key);
        }

        public void Remove(MetaLatchKey key)
        {
            Catalog.Remove(key);
        }

        public void TurnInAllKeys()
        {
            foreach (var key in Catalog)
            {
                key.Latch.TurnInKey(key);
            }
        }
    }
}
