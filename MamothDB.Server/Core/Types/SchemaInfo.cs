using Mamoth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamothDB.Server.Types
{
    public class SchemaInfo
    {
        public string FullLogicalPath { get; set; }
        public string LogicalParent { get; set; }
        public string Name { get; set; }
        public string ParentDiskPath { get; set; }
        public string FullDiskPath { get; set; }

        public string SchemaCatalog => System.IO.Path.Combine(FullDiskPath, Constants.FileNames.SchemaCatalog);
        public string ParentSchemaCatalog => System.IO.Path.Combine(ParentDiskPath, Constants.FileNames.SchemaCatalog);
        public string DocumentCatalog => System.IO.Path.Combine(FullDiskPath, Constants.FileNames.DocumentCatalog);
        public string ParentDocumentCatalog => System.IO.Path.Combine(ParentDiskPath, Constants.FileNames.DocumentCatalog);
    }
}
