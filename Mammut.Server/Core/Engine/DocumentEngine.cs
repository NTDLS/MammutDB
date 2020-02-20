using Mammut.Common.Payload.Model;
using Mammut.Server.Core.Models;
using Mammut.Server.Core.Models.Persist;
using Mammut.Server.Types;
using System;

namespace Mammut.Server.Core.Engine
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

            _core.IO.PutJson(session, documentFilePath, MetaDocument.FromPayload(document));

            collection.Add(MetaDocument.FromPayload(document));

            _core.IO.PutJson(session, schemaInfo.DocumentCatalog, collection);

            return new BasicDocumentInfo() { Id = document.Id };
        }

        /// <summary>
        /// Gets an existing document by its id.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public MetaDocument GetById(MetaSession session, string logicalSchemaPath, Guid documentId)
        {
            session.CurrentTransaction.AcquireSchemaLatch(logicalSchemaPath, Constants.LatchMode.Shared);

            var schemaInfo = _core.Schema.Parse(session, logicalSchemaPath);
            if (schemaInfo.Exists == false)
            {
                throw new Exception("The specified schema does not exist.");
            }

            var documentlogicalPath = schemaInfo.GetDocumentLogicalPath(documentId);
            session.CurrentTransaction.AcquireDocumentLatch(session, documentlogicalPath, Constants.LatchMode.Shared);

            var collection = _core.IO.GetJson<MetaDocumentCollection>(session, schemaInfo.DocumentCatalog);
            if(collection.Catalog.Contains(documentId) == false)
            {
                throw new Exception("The specified document does not exist.");
            }

            var documentFilePath = schemaInfo.GetDocumentFileName(documentId);
            var document = _core.IO.GetJson<MetaDocument>(session, documentFilePath);

            return document;
        }
    }
}