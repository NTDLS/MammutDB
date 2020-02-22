﻿using Mammut.Server.Core.Models;
using Mammut.Server.Core.Models.Persist;
using Mammut.Server.Core.State;
using Mammut.Server.Types;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

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
        public Guid Create(Session session, string logicalSchemaPath, Common.Payload.Model.Document document)
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

            return document.Id;
        }

        /// <summary>
        /// Gets an existing document by its id.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public MetaDocument GetById(Session session, string logicalSchemaPath, Guid documentId)
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
            if (collection.Catalog.Contains(documentId) == false)
            {
                throw new Exception("The specified document does not exist.");
            }

            var documentFilePath = schemaInfo.GetDocumentFileName(documentId);
            var document = _core.IO.GetJson<MetaDocument>(session, documentFilePath);

            return document;
        }

        /// <summary>
        /// Gets existing document IDs by a condition.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public List<Guid> GetIdsByCondition(Session session, string logicalSchemaPath, ConditionExpression conditions)
        {
            List<Guid> resultIDs = new List<Guid>();

            session.CurrentTransaction.AcquireSchemaLatch(logicalSchemaPath, Constants.LatchMode.Shared);

            var schemaInfo = _core.Schema.Parse(session, logicalSchemaPath);
            if (schemaInfo.Exists == false)
            {
                throw new Exception("The specified schema does not exist.");
            }

            var collection = _core.IO.GetJson<MetaDocumentCollection>(session, schemaInfo.DocumentCatalog);

            var workingConditions = conditions.ToWorkingConditionExpression();

            foreach (var documentId in collection.Catalog)
            {
                var documentlogicalPath = schemaInfo.GetDocumentFileName(documentId);
                var documentFilePath = schemaInfo.GetDocumentFileName(documentId);

                session.CurrentTransaction.AcquireDocumentLatch(session, documentlogicalPath, Constants.LatchMode.Shared);

                var document = _core.IO.GetJson<MetaDocument>(session, documentFilePath);

                JObject jsonContent = JObject.Parse(document.Content);

                if (IsMatch(conditions, jsonContent))
                {
                }
            }

            //var documentFilePath = schemaInfo.GetDocumentFileName(documentId);
            //var document = _core.IO.GetJson<MetaDocument>(session, documentFilePath);

            return resultIDs;
        }

        bool IsMatch(ConditionExpression conditions, JObject documentContent)
        {
            foreach (var statement in conditions.Statements)
            {
            }

            foreach (var child in conditions.Children)
            {
            }

            return false;
        }


    }
}
