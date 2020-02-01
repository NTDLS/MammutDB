using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MamothDB.Server.Core.Models
{
    /// <summary>
    /// Represents a collection of schemas in a catalog.
    /// </summary>
    public class MetaSchemaCollection
    {
        List<MetaSchema> Collection = new List<MetaSchema>();
    }
}
