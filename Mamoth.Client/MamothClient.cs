using Mamoth.Client.API;
using Mamoth.Common.Payload.Model;
using System;
using System.Net.Http;

namespace Mamoth.Client
{
    public class MamothClient : IDisposable
    {
        public HttpClient Client { get; private set; }
        public LoginToken Token { get; set; }
        public Security Security { get; private set; }

        private void Initialize(string baseAddress, TimeSpan commandTimeout, string username = "", string password = "")
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(baseAddress);
            Client.Timeout = commandTimeout;

            Token = null;

            Security = new Security(this);

            if (string.IsNullOrWhiteSpace(username) == false)
            {
                Security.Login(username, password);
            }
        }

        #region ~CTor
        public void Dispose()
        {
            if (this.Token.IsValid)
            {
                this.Logout();
            }
        }

        public MamothClient(string baseAddress, TimeSpan commandTimeout)
        {
            Initialize(baseAddress, commandTimeout);
        }

        public MamothClient(string baseAddress)
        {
            Initialize(baseAddress, new TimeSpan(0, 0, 1, 0, 0));
        }

        public MamothClient(string baseAddress, string username, string password)
        {
            Initialize(baseAddress, new TimeSpan(0, 0, 1, 0, 0), username, password);

        }

        public MamothClient(string baseAddress, TimeSpan commandTimeout, string username, string password)
        {
            Initialize(baseAddress, commandTimeout, username, password);
        }

        #endregion

        public LoginToken Login(string username, string password)
        {
            return Security.Login(username, password);
        }

        public void Logout()
        {
            Security.Logout();
        }
    }
}
