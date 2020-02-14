using MamothDB.Server.Core.Models;
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

        public MetaTransaction Enlist(MetaSession session)
        {
            if (session.CurrentTransaction != null)
            {
                throw new Exception("Transaction is already open.");
            }
            return new MetaTransaction(session, false);
        }

        /// <summary>
        /// Implicit transactions are created automatically by the engine if an explicit one was not already enlisted.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public MetaTransaction EnlistImplicit(MetaSession session)
        {
            var transaction = new MetaTransaction(session, true);
            session.CurrentTransaction = transaction;
            return transaction;
        }
    }
}
