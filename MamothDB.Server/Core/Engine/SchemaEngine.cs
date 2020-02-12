using Mamoth.Common;
using Mamoth.Common.Payload.Model;
using Mamoth.Common.Types;
using MamothDB.Server.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
                InitializeNewSchemaDirectory(Parse(":"));
            }
        }

        /// <summary>
        /// Creates a new schema directory and creates all necessary files withing it.
        /// </summary>
        /// <param name="schemaInfo"></param>
        private void InitializeNewSchemaDirectory(SchemaInfo schemaInfo)
        {
            if (Directory.Exists(schemaInfo.ParentDiskPath) == false)
            {
                Directory.CreateDirectory(schemaInfo.ParentDiskPath);
            }
            if (Directory.Exists(schemaInfo.FullDiskPath) == false)
            {
                Directory.CreateDirectory(schemaInfo.FullDiskPath);
            }

            _core.IO.PutJsonCached(schemaInfo.SchemaCatalog, new MetaSchemaCollection());
            _core.IO.PutJsonCached(schemaInfo.DocumentCatalog, new MetaDocumentCollection());
        }

        /// <summary>
        /// Creates a new single schema and returns its name and ID.
        /// </summary>
        /// <param name="schemaPath"></param>
        /// <returns></returns>
        public Guid Create(string schema)
        {
            var schemaInfo = Parse(schema);

            //TODO: Check if the schema exists first.

            var collection = _core.IO.GetJsonCached<MetaSchemaCollection>(schemaInfo.ParentSchemaCatalog);

            if (collection.GetByName(schemaInfo.Name) != null)
            {
                throw new Exception("The schema already exists.");
            }

            var metaSchema = new MetaSchema()
            {
                Id = Guid.NewGuid(),
                Name = schemaInfo.Name
            };

            InitializeNewSchemaDirectory(schemaInfo);

            collection.Add(metaSchema);
            _core.IO.PutJsonCached(schemaInfo.ParentSchemaCatalog, collection);

            return Guid.Empty;
        }

        /// <summary>
        /// Splits a full schema into its path and name parts.
        /// </summary>
        /// <returns></returns>
        public SchemaInfo Parse(string schema)
        {
            schema = schema.Trim(new char[] { ':' }).Replace("::", ":");

            var parts = new SchemaInfo();

            int lastDelimiterIndex = schema.LastIndexOf(":");

            if (lastDelimiterIndex > 0)
            {
                parts.LogicalParent = schema.Substring(0, lastDelimiterIndex);
                parts.Name = schema.Substring(lastDelimiterIndex + 1);
                parts.FullLogicalPath = $"{parts.LogicalParent}:{parts.Name}".Replace("::", ":");
                parts.ParentDiskPath = System.IO.Path.Join(_core.Settings.SchemaPath, parts.LogicalParent.Replace(':', '\\'));
                parts.FullDiskPath = System.IO.Path.Join(parts.ParentDiskPath, parts.Name);
            }
            else {
                parts.Name = schema;
                parts.LogicalParent = "";
                parts.FullLogicalPath = schema;
                parts.ParentDiskPath = _core.Settings.SchemaPath;
                parts.FullDiskPath = System.IO.Path.Join(_core.Settings.SchemaPath, parts.Name);
            }

            return parts;
        }

    }
}