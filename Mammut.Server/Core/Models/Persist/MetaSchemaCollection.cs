using System;
using System.Collections.Generic;

namespace Mammut.Server.Core.Models.Persist
{
    /// <summary>
    /// Represents a collection of schemas in a catalog.
    /// </summary>
    [Serializable]
    public class MetaSchemaCollection
    {
        public List<MetaSchema> Catalog = new List<MetaSchema>();

        public void Add(MetaSchema meta)
        {
            Catalog.Add(meta);
        }

        public MetaSchema GetByName(string name)
        {
            name = name.ToLower();
            return Catalog.Find(o => o.Name.ToLower() == name);
        }

        public MetaSchema GetById(Guid id)
        {
            return Catalog.Find(o => o.Id == id);
        }
    }
}
