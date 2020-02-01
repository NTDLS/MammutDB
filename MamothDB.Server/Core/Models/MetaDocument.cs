using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MamothDB.Server.Core.Models
{
    /// <summary>
    /// Represents a single document.
    /// </summary>
    public class MetaDocument
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
