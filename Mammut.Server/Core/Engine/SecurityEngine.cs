using Mammut.Server.Core.Models.Persist;
using Mammut.Server.Core.State;
using System;
using System.Linq;

namespace Mammut.Server.Core.Engine
{
    public class SecurityEngine
    {
        private ServerCore _core;

        public SecurityEngine(ServerCore core)
        {
            _core = core;
        }

        public Common.Payload.Model.Session Login(Common.Payload.Model.Login login)
        {
            var loginConnection = _core.IO.GetJsonDirty<MetaLoginCollection>(_core.Settings.LoginFile);

            var foundLogin = (from o in loginConnection.Catalog
                              where o.Username.ToLower() == login.Username.ToLower() && o.PasswordHash.ToLower() == login.PasswordHash.ToLower()
                              select o).FirstOrDefault();

            if (foundLogin != null)
            {
                return Session.ToPayload(_core.Session.Add(foundLogin));
            }

            throw new Exception("Login failed.");
        }

        public void Logout(Session session)
        {
            _core.Session.Remove(session);
        }
    }
}