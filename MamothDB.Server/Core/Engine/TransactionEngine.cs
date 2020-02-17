using MamothDB.Server.Core.Models;
using System;
using System.IO;
using System.Linq;

namespace MamothDB.Server.Core.Engine
{
    public class TransactionEngine
    {
        private ServerCore _core;

        public TransactionEngine(ServerCore core)
        {
            _core = core;
            var transactionPaths = Directory.EnumerateDirectories(_core.Settings.TransactionPath).ToList();

            foreach (var transactionPath in transactionPaths)
            {
                var transactionId = Path.GetFileName(transactionPath);

                var transaction = new MetaTransaction(_core, Guid.Parse(transactionId));
                transaction.Rollback();
            }

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
