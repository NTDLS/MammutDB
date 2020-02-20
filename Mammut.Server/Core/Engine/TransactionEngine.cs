using Mammut.Server.Core.State;
using System;
using System.IO;
using System.Linq;

namespace Mammut.Server.Core.Engine
{
    public class TransactionEngine
    {
        private ServerCore _core;

        public TransactionEngine(ServerCore core)
        {
            _core = core;
        }

        /// <summary>
        /// Finds transactions that were left over from an unexpected shutdown and rolls them back.
        /// </summary>
        public void Recover()
        {
            var transactionPaths = Directory.EnumerateDirectories(_core.Settings.TransactionPath).ToList();

            if (transactionPaths.Count > 0)
            {
                _core.LogInformation($"Rolling back {transactionPaths.Count} transactions.");
            }

            foreach (var transactionPath in transactionPaths)
            {
                var transactionId = Path.GetFileName(transactionPath);
                _core.LogInformation($"Rolling back transaction: {transactionId}.");
                var transaction = new Transaction(_core, Guid.Parse(transactionId));
                transaction.Rollback();
            }
        }

        public void Enlist(Session session)
        {
            if (session.CurrentTransaction == null)
            {
                session.CurrentTransaction = new Transaction(_core, session, false);
            }
            session.CurrentTransaction.Enlist();
        }

        /// <summary>
        /// Implicit transactions are created automatically by the engine if an explicit one was not already enlisted.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public void EnlistImplicit(Session session)
        {
            if (session.CurrentTransaction == null)
            {
                session.CurrentTransaction = new Transaction(_core, session, true);
            }
            session.CurrentTransaction.Enlist();
        }

        public void Commit(Session session)
        {
            if (session.CurrentTransaction == null)
            {
                throw new Exception("No transaction is active.");
            }
            if (session.CurrentTransaction.Commit())
            {
                session.CurrentTransaction = null;
            }
        }

        public void Rollback(Session session)
        {
            if (session.CurrentTransaction == null)
            {
                throw new Exception("No transaction is active.");
            }
            session.CurrentTransaction.Rollback();
            session.CurrentTransaction = null;
        }
    }
}
