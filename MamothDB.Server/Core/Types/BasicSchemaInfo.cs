using System;
using System.Collections.Generic;
using System.Text;

namespace MamothDB.Server.Types
{
    public class BasicSchemaInfo
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string LogicalPath { get; set; } = string.Empty;
    }
}
