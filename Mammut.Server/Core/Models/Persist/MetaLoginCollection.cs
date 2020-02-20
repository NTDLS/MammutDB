using System;
using System.Collections.Generic;

namespace Mammut.Server.Core.Models.Persist
{
    /// <summary>
    /// Represents a collection of logins in a catalog.
    /// </summary>
    [Serializable]
    public class MetaLoginCollection
    {
        public List<MetaLogin> Catalog = new List<MetaLogin>();

        public void Add(MetaLogin meta)
        {
            Catalog.Add(meta);
        }

        public MetaLogin GetByName(string name)
        {
            name = name.ToLower();
            return Catalog.Find(o => o.Username.ToLower() == name);
        }

        public MetaLogin GetById(Guid id)
        {
            return Catalog.Find(o => o.Id == id);
        }
    }
}
