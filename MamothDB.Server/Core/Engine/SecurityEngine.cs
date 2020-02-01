using Mamoth.Common.Payload.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MamothDB.Server.Core.Engine
{
    public class SecurityEngine
    {
        private ServerCore _core;

        public SecurityEngine(ServerCore core)
        {
            _core = core;
        }

        public Session Login(Login login)
        {
            if (login.Username == "root" && login.PasswordHash == "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855")
            {

                var session = new Session()
                {
                    SessionId = Guid.NewGuid(),
                    LoginId = Guid.Empty
                };

                return session;
            }

            throw new Exception("Login failed.");
        }

        public void Logout(Guid sessionId)
        {
            //Logout
        }
    }
}