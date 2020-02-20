using System;
using System.Collections.Generic;
using static Mammut.Server.Core.Constants;

namespace Mammut.Server.Core.State
{
    /// <summary>
    /// Represents a collection of locks in a catalog.
    /// </summary>
    [Serializable]
    public class LatchCollection
    {
        Dictionary<string, Latch> Catalog = new Dictionary<string, Latch>();

        /// <summary>
        /// Adds new or gets an existig latch.
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="fullObjectPath"></param>
        /// <returns></returns>
        public Latch AddOrGet(ObjectType objectType, string logicalSchemaPath)
        {
            string lookupKey = Utility.FileSystemPathToKey(logicalSchemaPath);

            if (Catalog.ContainsKey(lookupKey))
            {
                return Catalog[lookupKey];
            }
            else
            {
                Latch latch = new Latch(logicalSchemaPath)
                {
                    ObjectType = objectType
                };

                Catalog.Add(lookupKey, latch);

                return latch;
            }
        }
    }
}

