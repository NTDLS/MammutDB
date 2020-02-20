using Mammut.Common.Payload.Request;
using Mammut.Common.Payload.Response;
using System.Threading.Tasks;

namespace Mammut.Client.API
{
    public class SchemaClient : MammutAPI
    {
        private MammutClient _client;
        private const string _apiBase = "api/Schema";

        public SchemaClient(MammutClient client)
            : base(client)
        {
            _client = client;
        }

        /// <summary>
        /// Creates all schemas in a linage.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public ActionResponseBase CreateAll(string logicalSchemaPath)
        {
            var action = new ActionRequestSchema(_client.Token.SessionId)
            {
                Path = logicalSchemaPath
            };

            return Submit<ActionRequestSchema, ActionResponseBase>($"{_apiBase}/CreateAll", action);
        }

        /// <summary>
        /// Creates all schemas in a linage.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public async Task<ActionResponseBase> CreateAllAsync(string logicalSchemaPath)
        {
            var action = new ActionRequestSchema(_client.Token.SessionId)
            {
                Path = logicalSchemaPath
            };

            return (await SubmitAsync<ActionRequestSchema, ActionResponseBase>($"{_apiBase}/CreateAll", action));
        }


        /// <summary>
        /// Creates a single schema returns its name and id.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public ActionResponseSchema Create(string logicalSchemaPath)
        {
            var action = new ActionRequestSchema(_client.Token.SessionId)
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
            var action = new ActionRequestSchema(_client.Token.SessionId)
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
            var action = new ActionRequestSchema(_client.Token.SessionId)
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
            var action = new ActionRequestSchema(_client.Token.SessionId)
            {
                Path = logicalSchemaPath
            };

            return (await SubmitAsync<ActionRequestSchema, ActionResponseSchema>($"{_apiBase}/Get", action));
        }

    }
}