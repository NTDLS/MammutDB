using Mamoth.Common.Payload.Model;
using MamothDB.Server.Core.Models;
using MamothDB.Server.Core.Models.Persist;
using MamothDB.Server.Types;
using System;
using System.IO;

namespace MamothDB.Server.Core.Engine
{
    public class DocumentEngine
    {
        private ServerCore _core;

        public DocumentEngine(ServerCore core)
        {
            _core = core;
        }

        /// <summary>
        /// Creates a new single document and returns its id.
        /// </summary>
        /// <param name="schemaPath"></param>
        /// <returns></returns>
        public BasicDocumentInfo Create(MetaSession session, string logicalSchemaPath, Document document)
        {
            session.CurrentTransaction.AcquireSchemaLatch(logicalSchemaPath, Constants.LatchMode.Shared);

            var schemaInfo = _core.Schema.Parse(session, logicalSchemaPath);
            if (schemaInfo.Exists == false)
            {
                throw new Exception("The specified schema does not exist.");
            }

            //We allow the document ID to be generated at the client side.
            if (document.Id == Guid.Empty)
            {
                document.Id = Guid.NewGuid();
            }

            var collection = _core.IO.GetJson<MetaDocumentCollection>(session, schemaInfo.DocumentCatalog);

            var documentFilePath = schemaInfo.GetDocumentFileName(document);

            _core.IO.PutJson(session, documentFilePath, document);

            collection.Add(MetaDocument.FromPayload(document));

            _core.IO.PutJson(session, schemaInfo.DocumentCatalog, collection);

            /*
            var existingSchema = collection.GetByName(schemaInfo.Name);
            if (existingSchema != null)
            {
                //No need to hassle the user with an exception, just return the schema ID if it already exists.
                return new BasicDocumentInfo { Id = existingSchema.Id, LogicalPath = schemaInfo.FullLogicalPath };
                //throw new Exception("The schema already exists.");
            }

            var metaSchema = new MetaSchema()
            {
                Id = Guid.NewGuid(),
                Name = schemaInfo.Name
            };


            collection.Add(metaSchema);
            _core.IO.PutJson(session, schemaInfo.ParentSchemaCatalog, collection);

            return new BasicDocumentInfo { Id = metaSchema.Id, LogicalPath = schemaInfo.FullLogicalPath };
            */

            return new BasicDocumentInfo();
        }

        /// <summary>
        /// Gets an existing document by its id.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public BasicDocumentInfo GetById(MetaSession session, string logicalSchemaPath, Guid documentId)
        {
            session.CurrentTransaction.AcquireSchemaLatch(logicalSchemaPath, Constants.LatchMode.Shared);

            var schemaInfo = _core.Schema.Parse(session, logicalSchemaPath);
            if (schemaInfo.Exists == false)
            {
                throw new Exception("The specified schema does not exist.");
            }

            /*

            var collection = _core.IO.GetJson<MetaSchemaCollection>(session, schemaInfo.ParentSchemaCatalog);

            var existingSchema = collection.GetByName(schemaInfo.Name);
            if (existingSchema != null)
            {
                return new BasicDocumentInfo { Id = existingSchema.Id, LogicalPath = schemaInfo.FullLogicalPath };
            }
            */

            return new BasicDocumentInfo();
        }
    }
}