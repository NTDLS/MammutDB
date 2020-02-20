using Mammut.Common.Payload.Request;
using Mammut.Common.Payload.Response;
using System.Threading.Tasks;

namespace Mammut.Client.API
{
    public class TransactionClient : MammutAPI
    {
        private MammutClient _client;
        private const string _apiBase = "api/Transaction";

        public TransactionClient(MammutClient client)
            : base(client)
        {
            _client = client;
        }

        /// <summary>
        /// Creates a new transaction on the server.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public ActionResponseBase Enlist()
        {
            return Submit<ActionRequestBase, ActionResponseBase>($"{_apiBase}/Enlist", new ActionRequestBase(_client.Token.SessionId));
        }

        /// <summary>
        /// Creates a new transaction on the server.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public async Task<ActionResponseBase> EnlistAsync()
        {
            return (await SubmitAsync<ActionRequestBase, ActionResponseBase>($"{_apiBase}/Enlist", new ActionRequestBase(_client.Token.SessionId)));
        }

        /// <summary>
        /// Commits an existing explicit transaction at the server.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public ActionResponseBase Commit()
        {
            return Submit<ActionRequestBase, ActionResponseBase>($"{_apiBase}/Commit", new ActionRequestBase(_client.Token.SessionId));
        }

        /// <summary>
        /// Commits an existing explicit transaction at the server.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public async Task<ActionResponseBase> CommitAsync()
        {
            return (await SubmitAsync<ActionRequestBase, ActionResponseBase>($"{_apiBase}/Commit", new ActionRequestBase(_client.Token.SessionId)));
        }

        /// <summary>
        /// Rolls back all changes inside of an existing explicit transaction at the server.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public ActionResponseBase Rollback()
        {
            return Submit<ActionRequestBase, ActionResponseBase>($"{_apiBase}/Rollback", new ActionRequestBase(_client.Token.SessionId));
        }

        /// <summary>
        /// Rolls back all changes inside of an existing explicit transaction at the server.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public async Task<ActionResponseBase> RollbackAsync()
        {
            return (await SubmitAsync<ActionRequestBase, ActionResponseBase>($"{_apiBase}/Rollback", new ActionRequestBase(_client.Token.SessionId)));
        }
    }
}
