using Mamoth.Common.Payload.Model;
using System;

namespace Mamoth.Client
{
    /*
    using (var client = new MamothClient("https://localhost:5001", "root", ""))
    {
        client.Schema.Create("AR");
        client.Schema.Create("AR:Sales");
        client.Schema.Create("AR:Sales:Orders");
        client.Schema.Create("AR:Sales:People");
        client.Schema.Create("AR:Sales:People:Terminated");
        client.Schema.Create("AR:Customers");
        client.Schema.Create("AR:Customers:Prospects");
        client.Schema.Create("AR:Customers:Contracts");

        client.Logout();
    }
    */
    public class MamothClient : MamothClientBase, IDisposable
    {
        public MamothClient(string baseAddress, TimeSpan commandTimeout)
            : base(baseAddress, commandTimeout)
        {
        }

        public MamothClient(string baseAddress)
            : base(baseAddress)
        {
        }

        public MamothClient(string baseAddress, string username, string password)
            : base(baseAddress, username, password)
        {
        }

        public MamothClient(string baseAddress, TimeSpan commandTimeout, string username, string password)
            : base(baseAddress, commandTimeout, username, password)
        {
        }

        public new LoginToken Login(string username, string password)
        {
            return base.Login(username, password);
        }

        public new  void Logout()
        {
            base.Security.Logout();
        }
    }
}
