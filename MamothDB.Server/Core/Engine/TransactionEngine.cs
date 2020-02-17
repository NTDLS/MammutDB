using MamothDB.Server.Core.Models;
using MamothDB.Server.Core.Models.Persist;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MamothDB.Server.Core.Engine
{
    public class TransactionEngine
    {
        private ServerCore _core;

        public TransactionEngine(ServerCore core)
        {
            _core = core;
        }

        public void Enlist(MetaSession session)
        {
            if (session.CurrentTransaction != null)
            {
                throw new Exception("Transaction is already open.");
            }
            session.CurrentTransaction = new MetaTransaction(_core, session, false);
        }

        /// <summary>
        /// Implicit transactions are created automatically by the engine if an explicit one was not already enlisted.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public void EnlistImplicit(MetaSession session)
        {
            if (session.CurrentTransaction == null)
            {
                session.CurrentTransaction = new MetaTransaction(_core, session, true);
            }
        }

        public void Commit(MetaSession session)
        {
            if (session.CurrentTransaction == null)
            {
                throw new Exception("No transaction is active.");
            }
            session.CurrentTransaction.Commit();
        }

        public void Rollback(MetaSession session)
        {
            if (session.CurrentTransaction == null)
            {
                throw new Exception("No transaction is active.");
            }
            session.CurrentTransaction.Rollback();
        }
    }
}
