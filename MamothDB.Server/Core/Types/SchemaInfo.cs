using Mamoth.Common.Payload.Model;
using MamothDB.Server.Core;
using MamothDB.Server.Core.Models.Persist;
using System;
using System.IO;

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

        public string GetDocumentFileName(Guid documentId) => Path.Combine(FullDiskPath, documentId.ToString()) + Constants.Filesystem.DocumentFileExtention;
        public string GetDocumentFileName(Document document) => GetDocumentFileName(document.Id);
        public string GetDocumentFileName(MetaDocument document) => GetDocumentFileName(document.Id);

        public bool Exists
        {
            get
            {
                return File.Exists(SchemaCatalog);
            }
        }
    }
}
