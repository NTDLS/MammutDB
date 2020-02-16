﻿using Mamoth.Common;
using Mamoth.Common.Payload.Model;
using MamothDB.Server.Core.Models;
using MamothDB.Server.Core.Models.Persist;
using MamothDB.Server.Types;
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
                InitializeNewSchemaDirectoryDirty(Parse(":"));
            }
        }

        /// <summary>
        /// //This shoule only be called when creating the initial structure at first ever server startup.
        /// </summary>
        /// <param name="schemaInfo"></param>
        private void InitializeNewSchemaDirectoryDirty(SchemaInfo schemaInfo)
        {
            if (Directory.Exists(schemaInfo.ParentDiskPath) == false)
            {
                Directory.CreateDirectory(schemaInfo.ParentDiskPath);
            }
            if (Directory.Exists(schemaInfo.FullDiskPath) == false)
            {
                Directory.CreateDirectory(schemaInfo.FullDiskPath);
            }

            _core.IO.PutJsonDirty(schemaInfo.SchemaCatalog, new MetaSchemaCollection());
            _core.IO.PutJsonDirty(schemaInfo.DocumentCatalog, new MetaDocumentCollection());
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

            var schemaInfo = Parse(logicalSchemaPath);

            var collection = _core.IO.GetJson<MetaSchemaCollection>(session, schemaInfo.ParentSchemaCatalog);

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

            InitializeNewSchemaDirectoryDirty(schemaInfo);

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

            var schemaInfo = Parse(logicalSchemaPath);

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
        public SchemaInfo Parse(string logicalSchemaPath)
        {
            logicalSchemaPath = logicalSchemaPath.Trim(new char[] { ':' }).Replace("::", ":");

            var parts = new SchemaInfo();

            int lastDelimiterIndex = logicalSchemaPath.LastIndexOf(":");

            if (lastDelimiterIndex > 0)
            {
                parts.LogicalParent = logicalSchemaPath.Substring(0, lastDelimiterIndex);
                parts.Name = logicalSchemaPath.Substring(lastDelimiterIndex + 1);
                parts.FullLogicalPath = $"{parts.LogicalParent}:{parts.Name}".Replace("::", ":");
                parts.ParentDiskPath = System.IO.Path.Join(_core.Settings.SchemaPath, parts.LogicalParent.Replace(':', '\\'));
                parts.FullDiskPath = System.IO.Path.Join(parts.ParentDiskPath, parts.Name);
            }
            else {
                parts.Name = logicalSchemaPath;
                parts.LogicalParent = "";
                parts.FullLogicalPath = logicalSchemaPath;
                parts.ParentDiskPath = _core.Settings.SchemaPath;
                parts.FullDiskPath = System.IO.Path.Join(_core.Settings.SchemaPath, parts.Name);
            }

            return parts;
        }
    }
}