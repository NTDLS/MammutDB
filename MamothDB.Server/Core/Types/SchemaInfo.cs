using Mamoth.Common;
using MamothDB.Server.Core;
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
        public string SchemaCatalog => System.IO.Path.Combine(FullDiskPath, Constants.Filesystem.SchemaCatalog);
        public string ParentSchemaCatalog => System.IO.Path.Combine(ParentDiskPath, Constants.Filesystem.SchemaCatalog);
        public string DocumentCatalog => System.IO.Path.Combine(FullDiskPath, Constants.Filesystem.DocumentCatalog);
        public string ParentDocumentCatalog => System.IO.Path.Combine(ParentDiskPath, Constants.Filesystem.DocumentCatalog);

        private string _objectKey = string.Empty;
        public string ObjectKey
        {
            get
            {
                if (string.IsNullOrEmpty(_objectKey))
                {
                    _objectKey = Utility.FileSystemPathToKey(FullLogicalPath);
                }
                return _objectKey;
            }
        }

    }
}