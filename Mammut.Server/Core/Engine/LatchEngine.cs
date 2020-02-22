using Mammut.Server.Core.State;
using static Mammut.Server.Core.Constants;

namespace Mammut.Server.Core.Engine
{
    public class LatchEngine
    {
        private ServerCore _core;

        private LatchCollection _latches;

        public LatchEngine(ServerCore core)
        {
            _core = core;

            _latches = new LatchCollection();
        }

        /// <summary>
        /// Places a latch (or lock) on an object with the specified mode (shared or exclusive).
        /// If you plan on initially using a shared lock then potentiall converting to an exclusive, it is generally safer to acquire the exclusive up front.
        /// Latches are released then the transaction is terminated by commit or rollback.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="logicalSchemaPath"></param>
        /// <param name="latchMode"></param>
        public void AcquireSchemaLatch(Session session, string logicalSchemaPath, LatchMode latchMode)
        {
            lock (CriticalSections.AcquireLock)
            {
                AcquireSingleSchemaLatch(session, logicalSchemaPath, latchMode);

                var schemaInfo = _core.Schema.Parse(session, logicalSchemaPath);

                //Place shared locks on all parents of the schema,
                schemaInfo = _core.Schema.Parse(session, schemaInfo.LogicalParent);
                while (string.IsNullOrEmpty(schemaInfo.Name) == false)
                {
                    AcquireSingleSchemaLatch(session, schemaInfo.FullLogicalPath, LatchMode.Shared);
                    schemaInfo = _core.Schema.Parse(session, schemaInfo.LogicalParent);
                }
            }
        }

        private void AcquireSingleSchemaLatch(Session session, string logicalSchemaPath, LatchMode latchMode)
        {
            lock (CriticalSections.AcquireLock)
            {
                //TODO: Lookup existing latches to see if there are any that are incompatible with this one.
                //...
                //...
                //...

                //Get or add a new latch on the object.
                var latch = _latches.AddOrGet(Constants.ObjectType.Schema, logicalSchemaPath);

                //Get a new key to the object.
                var latchKey = latch.IssueKey(session.CurrentTransaction, latchMode);

                //Give the latch key to the current transaction.
                session.CurrentTransaction.AddLatchKey(latchKey);
            }
        }


        /// <summary>
        /// Places a latch (or lock) on an object with the specified mode (shared or exclusive).
        /// If you plan on initially using a shared lock then potentiall converting to an exclusive, it is generally safer to acquire the exclusive up front.
        /// Latches are released then the transaction is terminated by commit or rollback.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="logicalDocumentPath"></param>
        /// <param name="latchMode"></param>
        public void AcquireDocumentLatch(Session session, string logicalDocumentPath, LatchMode latchMode)
        {
            lock (CriticalSections.AcquireLock)
            {
                var schemaInfo = _core.Schema.Parse(session, logicalDocumentPath);

                //Place shared locks on all parents of the document.
                schemaInfo = _core.Schema.Parse(session, schemaInfo.LogicalParent);
                while (string.IsNullOrEmpty(schemaInfo.Name) == false)
                {
                    AcquireSingleSchemaLatch(session, schemaInfo.FullLogicalPath, LatchMode.Shared);
                    schemaInfo = _core.Schema.Parse(session, schemaInfo.LogicalParent);
                }

                //Get or add a new latch on the object.
                var latch = _latches.AddOrGet(Constants.ObjectType.Document, logicalDocumentPath);

                //Get a new key to the object.
                var latchKey = latch.IssueKey(session.CurrentTransaction, latchMode);

                //Give the latch key to the current transaction.
                session.CurrentTransaction.AddLatchKey(latchKey);
            }
        }
    }
}
