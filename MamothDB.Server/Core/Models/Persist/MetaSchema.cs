using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MamothDB.Server.Core.Models.Persist
{
    /// <summary>
    /// Represents a schema.
    /// </summary>
    public class MetaSchema
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
