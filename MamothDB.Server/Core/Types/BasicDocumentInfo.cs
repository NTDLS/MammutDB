using System;

namespace MamothDB.Server.Types
{
    public class BasicDocumentInfo
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string LogicalPath { get; set; } = string.Empty;
    }
}
