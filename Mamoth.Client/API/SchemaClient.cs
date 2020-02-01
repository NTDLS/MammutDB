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
        private MamothClient _client;
        private const string _apiBase = "api/Schema";

        public SchemaClient(MamothClient client)
            : base(client)
        {
            _client = client;
        }

        /// <summary>
        /// Creates a single schema returns its name and id.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public ActionResponseSchema Create(string schema)
        {
            var action = new ActionRequestSchema()
            {
                Name = schema
            };

            return Submit<ActionRequestSchema, ActionResponseSchema>($"{_apiBase}/Create", action);
        }

        /// <summary>
        /// Creates a single schema returns its name and id.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public async Task<ActionResponseSchema> CreateAsync(string schema)
        {
            var action = new ActionRequestSchema()
            {
                Name = schema
            };

            return (await SubmitAsync<ActionRequestSchema, ActionResponseSchema>($"{_apiBase}/Create", action));
        }
    }
}