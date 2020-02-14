using Mamoth.Common.Payload.Model;
using MamothDB.Server.Core.Models;
using MamothDB.Server.Core.Models.Persist;
using System;
using System.Linq;

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
            var loginConnection = _core.IO.GetJsonDirty<MetaLoginCollection>(_core.Settings.LoginFile);

            var foundLogin = (from o in loginConnection.Catalog
                              where o.Username.ToLower() == login.Username.ToLower() && o.PasswordHash.ToLower() == login.PasswordHash.ToLower()
                              select o).FirstOrDefault();

            if (foundLogin != null)
            {
                return MetaSession.ToPayload(_core.Session.Add(foundLogin));
            }

            throw new Exception("Login failed.");
        }

        public void Logout(MetaSession session)
        {
            _core.Session.Remove(session);
        }
    }
}