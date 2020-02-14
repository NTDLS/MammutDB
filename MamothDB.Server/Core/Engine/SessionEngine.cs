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
    }
}