using Mammut.Common.Payload.Model;
using Mammut.Common.Payload.Request;
using Mammut.Common.Payload.Response;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Mammut.Client.API
{
    public class QueryClient : MammutAPI
    {
        private MammutClient _client;
        private const string _apiBase = "api/Query";

        public QueryClient(MammutClient client)
            : base(client)
        {
            _client = client;
        }

        public ActionResponseBase ExecuteDummy()
        {
            var action = new ActionRequestBase(_client.Token.SessionId);

            return Submit<ActionRequestBase, ActionResponseBase>($"{_apiBase}/ExecuteDummy", action);
        }
    }
}