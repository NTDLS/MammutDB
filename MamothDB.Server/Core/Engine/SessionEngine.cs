using Mamoth.Common;
using Mamoth.Common.Payload.Model;
using MamothDB.Server.Core.Models;
using MamothDB.Server.Core.Models.Persist;
using MamothDB.Server.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MamothDB.Server.Core.Engine
{
    public class SessionEngine
    {
        private ServerCore _core;

        private MetaSessionCollection _collection = new MetaSessionCollection();

        public SessionEngine(ServerCore core)
        {
            _core = core;
        }

        public MetaSession Add(MetaLogin login)
        {
            var session = new MetaSession()
            {
                LoginId = login.Id,
                Username = login.Username,
                SessionId = Guid.NewGuid()
            };

            _collection.Add(session);

            return session;
        }

        public void Remove(MetaSession session)
        {
            _collection.Catalog.Remove(session);
        }

        /// <summary>
        /// Gets a session by its id and enlists in implicit transaction if one is not already open. Throws an exception if session is not found.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public MetaSession ObtainSession(Guid sessionId, bool createImplitTransaction = true)
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
