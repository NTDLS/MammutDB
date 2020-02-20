using Mammut.Server.Core.Models.Persist;
using Mammut.Server.Core.State;
using System;

namespace Mammut.Server.Core.Engine
{
    public class SessionEngine
    {
        private ServerCore _core;

        private SessionCollection _collection = new SessionCollection();

        public SessionEngine(ServerCore core)
        {
            _core = core;
        }

        public Session Add(MetaLogin login)
        {
            var session = new Session()
            {
                LoginId = login.Id,
                Username = login.Username,
                SessionId = Guid.NewGuid()
            };

            _collection.Add(session);

            return session;
        }

        public void Remove(Session session)
        {
            _collection.Catalog.Remove(session);
        }

        /// <summary>
        /// Gets a session by its id and enlists in implicit transaction if one is not already open. Throws an exception if session is not found.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public Session ObtainSession(Guid sessionId, bool createImplitTransaction = true)
        {
            var session = _collection.Catalog.Find(o => o.SessionId == sessionId);
            if (session == null)
            {
                throw new Exception("Invalid session.");
            }

            if (createImplitTransaction && session.CurrentTransaction == null)
            {
                _core.Transaction.EnlistImplicit(session);
            }

            return session;
        }
    }
}
