using Mamoth.Common;
using Mamoth.Common.Payload.Model;
using Mamoth.Common.Payload.Request;
using Mamoth.Common.Payload.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mamoth.Client.API
{
    public class SchemaClient : MamothBase
    {
        private MamothClientBase _client;
        private const string _apiBase = "api/Schema";

        public SchemaClient(MamothClientBase client)
            : base(client)
        {
            _client = client;
        }

        /// <summary>
        /// Creates a single schema returns its name and id.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public ActionResponseSchema Create(string logicalSchemaPath)
        {
            var action = new ActionRequestSchema()
            {
                Path = logicalSchemaPath
            };

            return Submit<ActionRequestSchema, ActionResponseSchema>($"{_apiBase}/Create", action);
        }

        /// <summary>
        /// Creates a single schema returns its name and id.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public async Task<ActionResponseSchema> CreateAsync(string logicalSchemaPath)
        {
            var action = new ActionRequestSchema()
            {
                Path = logicalSchemaPath
            };

            return (await SubmitAsync<ActionRequestSchema, ActionResponseSchema>($"{_apiBase}/Create", action));
        }

        /// <summary>
        /// Gets a schema by its name.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public ActionResponseSchema Get(string logicalSchemaPath)
        {
            var action = new ActionRequestSchema()
            {
                Path = logicalSchemaPath
            };

            return Submit<ActionRequestSchema, ActionResponseSchema>($"{_apiBase}/Get", action);
        }

        /// <summary>
        /// /// Gets a schema by its name.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public async Task<ActionResponseSchema> GetAsync(string logicalSchemaPath)
        {
            var action = new ActionRequestSchema()
            {
                Path = logicalSchemaPath
            };

            return (await SubmitAsync<ActionRequestSchema, ActionResponseSchema>($"{_apiBase}/Get", action));
        }

    }
}