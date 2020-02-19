using MamothDB.Server.Core.Models;
using MamothDB.Server.Core.Models.Persist;
using MamothDB.Server.Types;
using System;
using System.IO;

namespace MamothDB.Server.Core.Engine
{
    public class SchemaEngine
    {
        private ServerCore _core;

        public SchemaEngine(ServerCore core)
        {
            _core = core;

            if (Directory.Exists(_core.Settings.SchemaPath) == false)
            {
                Directory.CreateDirectory(_core.Settings.SchemaPath);
            }

            string schemaCatalog = Path.Combine(_core.Settings.SchemaPath, Constants.Filesystem.SchemaCatalog);
            if (File.Exists(schemaCatalog) == false)
            {
                _core.IO.PutJsonDirty(schemaCatalog, new MetaSchemaCollection());
            }

            string documentCatalog = Path.Combine(_core.Settings.SchemaPath, Constants.Filesystem.DocumentCatalog);
            if (File.Exists(documentCatalog) == false)
            {
                _core.IO.PutJsonDirty(documentCatalog, new MetaDocumentCollection());
            }
        }

        private void InitializeNewSchemaDirectory(MetaSession session, SchemaInfo schemaInfo)
        {
            if (_core.IO.DirectoryExists(session, schemaInfo.ParentDiskPath) == false)
            {
                _core.IO.CreateDirectory(session, schemaInfo.ParentDiskPath);
            }
            if (_core.IO.DirectoryExists(session, schemaInfo.FullDiskPath) == false)
            {
                _core.IO.CreateDirectory(session, schemaInfo.FullDiskPath);
            }

            _core.IO.PutJson(session, schemaInfo.SchemaCatalog, new MetaSchemaCollection());
            _core.IO.PutJson(session, schemaInfo.DocumentCatalog, new MetaDocumentCollection());
        }

        /// <summary>
        /// Creates a new single schema and returns its name and ID.
        /// </summary>
        /// <param name="schemaPath"></param>
        /// <returns></returns>
        public BasicSchemaInfo Create(MetaSession session, string logicalSchemaPath)
        {
            session.CurrentTransaction.AcquireSchemaLatch(logicalSchemaPath, Constants.LatchMode.Exclusive);

            var schemaInfo = Parse(session, logicalSchemaPath);

            var parentSchemaInfo = Parse(session, schemaInfo.LogicalParent);
            if (parentSchemaInfo.Exists == false)
            {
                throw new Exception("The specified parent schema does not exist.");
            }

            var collection = _core.IO.GetJson<MetaSchemaCollection>(session, parentSchemaInfo.SchemaCatalog);

            var existingSchema = collection.GetByName(schemaInfo.Name);
            if (existingSchema != null)
            {
                //No need to hassle the user with an exception, just return the schema ID if it already exists.
                return new BasicSchemaInfo { Id = existingSchema.Id, Name = existingSchema.Name, LogicalPath = schemaInfo.FullLogicalPath };
                //throw new Exception("The schema already exists.");
            }

            var metaSchema = new MetaSchema()
            {
                Id = Guid.NewGuid(),
                Name = schemaInfo.Name
            };

            InitializeNewSchemaDirectory(session, schemaInfo);

            collection.Add(metaSchema);
            _core.IO.PutJson(session, schemaInfo.ParentSchemaCatalog, collection);

            return new BasicSchemaInfo { Id = metaSchema.Id, Name = metaSchema.Name, LogicalPath = schemaInfo.FullLogicalPath };
        }

        /// <summary>
        /// Gets an existing schema name and id by name.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public BasicSchemaInfo Get(MetaSession session, string logicalSchemaPath)
        {
            session.CurrentTransaction.AcquireSchemaLatch(logicalSchemaPath, Constants.LatchMode.Exclusive);

            var schemaInfo = Parse(session, logicalSchemaPath);
            if (schemaInfo.Exists == false)
            {
                throw new Exception("The specified schema does not exist.");
            }

            var collection = _core.IO.GetJson<MetaSchemaCollection>(session, schemaInfo.ParentSchemaCatalog);

            var existingSchema = collection.GetByName(schemaInfo.Name);
            if (existingSchema != null)
            {
                return new BasicSchemaInfo { Id = existingSchema.Id, Name = existingSchema.Name, LogicalPath = schemaInfo.FullLogicalPath };
            }

            return new BasicSchemaInfo();
        }

        /// <summary>
        /// Splits a full schema into its path and name parts.
        /// </summary>
        /// <returns></returns>
        public SchemaInfo Parse(MetaSession session, string logicalSchemaPath)
        {
            logicalSchemaPath = logicalSchemaPath.Trim(new char[] { ':' }).Replace("::", ":");

            var parts = new SchemaInfo(_core, session);

            int lastDelimiterIndex = logicalSchemaPath.LastIndexOf(":");

            if (lastDelimiterIndex > 0)
            {
                parts.LogicalParent = logicalSchemaPath.Substring(0, lastDelimiterIndex);
                parts.Name = logicalSchemaPath.Substring(lastDelimiterIndex + 1);
                parts.FullLogicalPath = $"{parts.LogicalParent}:{parts.Name}".Replace("::", ":");
                parts.ParentDiskPath = Path.Join(_core.Settings.SchemaPath, parts.LogicalParent.Replace(':', '\\'));
                parts.FullDiskPath = Path.Join(parts.ParentDiskPath, parts.Name);
            }
            else {
                parts.Name = logicalSchemaPath;
                parts.LogicalParent = "";
                parts.FullLogicalPath = logicalSchemaPath;
                parts.ParentDiskPath = _core.Settings.SchemaPath;
                parts.FullDiskPath = Path.Join(_core.Settings.SchemaPath, parts.Name);
            }

            return parts;
        }
    }
}