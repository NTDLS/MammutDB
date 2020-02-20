using System;
using System.Collections.Generic;

namespace Mammut.Server.Core.State
{
    /// <summary>
    /// Represents a collection of keys in a catalog.
    /// </summary>
    [Serializable]
    public class LatchKeyCollection
    {
        private List<LatchKey> Catalog = new List<LatchKey>();

        public LatchKeyCollection()
        {
        }

        public void Add(LatchKey key)
        {
            Catalog.Add(key);
        }

        public void Remove(LatchKey key)
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
