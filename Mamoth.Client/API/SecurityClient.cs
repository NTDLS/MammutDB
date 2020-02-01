using Mamoth.Common;
using Mamoth.Common.Payload.Model;
using Mamoth.Common.Payload.Request;
using Mamoth.Common.Payload.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mamoth.Client.API
{
    public class SecurityClient : MamothBase
    {
        private MamothClient _client;
        private const string _apiBase = "api/Security";

        public SecurityClient(MamothClient client)
            : base(client)
        {
            _client = client;
        }

        /// <summary>
        /// Logs a user into the server.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public LoginToken Login(string username, string password)
        {
            var action = new ActionRequestLogin()
            {
                Login = new Login(username, password)
            };

            _client.Token = Submit<ActionRequestLogin, ActionResponceLogin>($"{_apiBase}/Login", action).ToToken();

            return _client.Token;
        }

        /// <summary>
        /// Logs a user out of the server.
        /// </summary>
        public void Logout()
        {
            Submit<ActionRequestBase, ActionResponseBase>
                ($"{_apiBase}/Logout", new ActionRequestBase(_client.Token.SessionId));

            _client.Token = new LoginToken();
        }

        /// <summary>
        /// Gets a list of all logins from the server.
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetLoginsAsync()
        {

            return (await SubmitAsync<ActionRequestBase, ActionResponceUsers>
                ($"{_apiBase}/ListUsers", new ActionRequestBase(_client.Token.SessionId))).List;
        }

        public List<User> GetLogins()
        {
            return Submit<ActionRequestBase, ActionResponceUsers>
                ($"{_apiBase}/ListUsers", new ActionRequestBase(_client.Token.SessionId)).List;
        }

        public async Task<Guid> CreatUserAsync(string username, string passwordHash)
        {
            var action = new ActionRequestLogin(_client.Token.SessionId)
            {
                Login = new Login(username, passwordHash)
            };

            return (await SubmitAsync<ActionRequestLogin, ActionResponseId>
                ($"{_apiBase}/CreatUser", action)).Id;
        }

        public Guid CreatUser(string username, string passwordHash)
        {
            var action = new ActionRequestLogin(_client.Token.SessionId)
            {
                Login = new Login(username, passwordHash)
            };

            return Submit<ActionRequestLogin, ActionResponseId>
                ($"{_apiBase}/CreatUser", action).Id;
        }

        public async Task<Guid> SetLoginPasswordAsync(string username, string passwordHash)
        {
            var action = new ActionRequestLogin(_client.Token.SessionId)
            {
                Login = new Login(username, passwordHash)
            };

            return (await SubmitAsync<ActionRequestLogin, ActionResponseId>
                ($"{_apiBase}/SetLoginPasswordByName", action)).Id;
        }

        public Guid SetLoginPassword(string username, string passwordHash)
        {
            var action = new ActionRequestLogin(_client.Token.SessionId)
            {
                Login = new Login(username, passwordHash)
            };

            return Submit<ActionRequestLogin, ActionResponseId>
                ($"{_apiBase}/SetLoginPasswordByName", action).Id;
        }

        public async Task DeletUserByNameAsync(string username)
        {
            var action = new ActionRequestGenericObject(_client.Token.SessionId)
            {
                ObjectName = username
            };

            await SubmitAsync<ActionRequestGenericObject, ActionResponseId>
                ($"{_apiBase}/DeletUserByName", action);
        }

        public void DeletUserByName(string username)
        {
            var action = new ActionRequestGenericObject(_client.Token.SessionId)
            {
                ObjectName = username
            };

            Submit<ActionRequestGenericObject, ActionResponseId>
                ($"{_apiBase}/DeletUserByName", action);
        }
    }
}
