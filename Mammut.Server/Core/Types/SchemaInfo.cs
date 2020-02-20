using Mammut.Server.Core;
using Mammut.Server.Core.Models.Persist;
using Mammut.Server.Core.State;
using System;
using System.IO;

namespace Mammut.Server.Types
{
    public class SchemaInfo
    {
        private ServerCore _core;
        private Session _session;

        public SchemaInfo(ServerCore core, Session session)
        {
            _core = core;
            _session = session;
        }

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

        public string GetDocumentLogicalPath(Guid documentId) => $"{FullLogicalPath}:{documentId.ToString()}";
        public string GetDocumentLogicalPath(Common.Payload.Model.Document document) => GetDocumentLogicalPath(document.Id);
        public string GetDocumentLogicalPath(MetaDocument document) => GetDocumentLogicalPath(document.Id);


        public string GetDocumentFileName(Guid documentId) => Path.Combine(FullDiskPath, documentId.ToString()) + Constants.Filesystem.DocumentFileExtention;
        public string GetDocumentFileName(Common.Payload.Model.Document document) => GetDocumentFileName(document.Id);
        public string GetDocumentFileName(MetaDocument document) => GetDocumentFileName(document.Id);

        public bool Exists
        {
            get
            {
                return _core.IO.FileExists(_session, SchemaCatalog);
            }
        }
    }
}
