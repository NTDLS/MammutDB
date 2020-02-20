using System;

namespace Mammut.Server.Core.Models.Persist
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
