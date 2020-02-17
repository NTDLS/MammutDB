using System;
using System.Collections.Generic;
using static MamothDB.Server.Core.Constants;

namespace MamothDB.Server.Core.Models
{
    /// <summary>
    /// Represents a collection of locks in a catalog.
    /// </summary>
    [Serializable]
    public class MetaLatchCollection
    {
        Dictionary<string, MetaLatch> Catalog = new Dictionary<string, MetaLatch>();

        /// <summary>
        /// Adds new or gets an existig latch.
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="fullObjectPath"></param>
        /// <returns></returns>
        public MetaLatch AddOrGet(ObjectType objectType, string logicalSchemaPath)
        {
            string lookupKey = Utility.FileSystemPathToKey(logicalSchemaPath);

            if (Catalog.ContainsKey(lookupKey))
            {
                return Catalog[lookupKey];
            }
            else
            {
                MetaLatch latch = new MetaLatch(logicalSchemaPath)
                {
                    ObjectType = objectType
                };

                Catalog.Add(lookupKey, latch);

                return latch;
            }
        }
    }
}

