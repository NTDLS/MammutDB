using Mamoth.Common.Payload.Model;
using Mamoth.Common.Payload.Request;
using Mamoth.Common.Payload.Response;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Mamoth.Client.API
{
    public class DocumentClient : MamothAPI
    {
        private MamothClient _client;
        private const string _apiBase = "api/Document";

        public DocumentClient(MamothClient client)
            : base(client)
        {
            _client = client;
        }

        /// <summary>
        /// Creates a single document returns its id.
        /// </summary>
        /// <param name="logicalSchemaPath"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public ActionResponseDocument Create(string logicalSchemaPath, Document document)
        {
            var action = new ActionRequestDocument(_client.Token.SessionId)
            {
                Path = logicalSchemaPath,
                Document = document
            };

            return Submit<ActionRequestDocument, ActionResponseDocument>($"{_apiBase}/Create", action);
        }

        public ActionResponseDocument Create(string logicalSchemaPath, object documentContent)
        {
            var document = new Document()
            {
                Text = JsonConvert.SerializeObject(documentContent)
            };

            var action = new ActionRequestDocument(_client.Token.SessionId)
            {
                Path = logicalSchemaPath,
                Document = document
            };

            return Submit<ActionRequestDocument, ActionResponseDocument>($"{_apiBase}/Create", action);
        }

        /// <summary>
        /// Creates a single document returns its id.
        /// </summary>
        /// <param name="logicalSchemaPath"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public async Task<ActionResponseDocument> CreateAsync(string logicalSchemaPath, Document document)
        {
            var action = new ActionRequestDocument(_client.Token.SessionId)
            {
                Path = logicalSchemaPath,
                Document = document
            };

            return (await SubmitAsync<ActionRequestDocument, ActionResponseDocument>($"{_apiBase}/Create", action));
        }

        public async Task<ActionResponseDocument> CreateAsync(string logicalSchemaPath, object documentContent)
        {
            var document = new Document()
            {
                Text = JsonConvert.SerializeObject(documentContent)
            };

            var action = new ActionRequestDocument(_client.Token.SessionId)
            {
                Path = logicalSchemaPath,
                Document = document
            };

            return (await SubmitAsync<ActionRequestDocument, ActionResponseDocument>($"{_apiBase}/Create", action));
        }

        /// <summary>
        /// Gets a document by its id.
        /// </summary>
        /// <param name="logicalSchemaPath"></param>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public ActionResponseDocument Get(string logicalSchemaPath, Guid documentId)
        {
            var action = new ActionRequestDocument(_client.Token.SessionId)
            {
                Path = logicalSchemaPath,
                Id = documentId
            };

            return Submit<ActionRequestDocument, ActionResponseDocument>($"{_apiBase}/Get", action);
        }

        /// <summary>
        /// /// Gets a document by its id.
        /// </summary>
        /// <param name="logicalSchemaPath"></param>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public async Task<ActionResponseDocument> GetAsync(string logicalSchemaPath, Guid documentId)
        {
            var action = new ActionRequestDocument(_client.Token.SessionId)
            {
                Path = logicalSchemaPath,
                Id = documentId
            };

            return (await SubmitAsync<ActionRequestDocument, ActionResponseDocument>($"{_apiBase}/Get", action));
        }
    }
}