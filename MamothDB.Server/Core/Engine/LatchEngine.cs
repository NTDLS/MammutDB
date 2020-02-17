using MamothDB.Server.Core.Models;
using static MamothDB.Server.Core.Constants;

namespace MamothDB.Server.Core.Engine
{
    public class LatchEngine
    {
        private ServerCore _core;

        private MetaLatchCollection _latches;

        public LatchEngine(ServerCore core)
        {
            _core = core;

            _latches = new MetaLatchCollection();
        }

        /// <summary>
        /// Places a latch (or lock) on an object with the specified mode (shared or exclusive).
        /// If you plan on initially using a shared lock then potentiall converting to an exclusive, it is generally safer to acquire the exclusive up front.
        /// Latches are released then the transaction is terminated by commit or rollback.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="logicalSchemaPath"></param>
        /// <param name="latchMode"></param>
        public void AcquireSchemaLatch(MetaSession session, string logicalSchemaPath, LatchMode latchMode)
        {
            AcquireSingleSchemaLatch(session, logicalSchemaPath, latchMode);

            var schemaInfo = _core.Schema.Parse(logicalSchemaPath);

            //Place shared locks on all parents of the schema,
            schemaInfo = _core.Schema.Parse(schemaInfo.LogicalParent);
            while (string.IsNullOrEmpty(schemaInfo.Name) == false)
            {
                AcquireSingleSchemaLatch(session, schemaInfo.FullLogicalPath, LatchMode.Shared);
                schemaInfo = _core.Schema.Parse(schemaInfo.LogicalParent);
            } 
        }

        private void AcquireSingleSchemaLatch(MetaSession session, string logicalSchemaPath, LatchMode latchMode)
        {
            //Get or add a new latch on the object.
            var latch = _latches.AddOrGet(Constants.ObjectType.Schema, logicalSchemaPath);

            //Get a new key to the object.
            var latchKey = latch.IssueKey(session.CurrentTransaction, latchMode);

            //Give the latch key to the current transaction.
            session.CurrentTransaction.AddLatchKey(latchKey);
        }


        /// <summary>
        /// Places a latch (or lock) on an object with the specified mode (shared or exclusive).
        /// If you plan on initially using a shared lock then potentiall converting to an exclusive, it is generally safer to acquire the exclusive up front.
        /// Latches are released then the transaction is terminated by commit or rollback.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="logicalDocumentPath"></param>
        /// <param name="latchMode"></param>
        public void AcquireDocumentLatch(MetaSession session, string logicalDocumentPath, LatchMode latchMode)
        {
            var schemaInfo = _core.Schema.Parse(logicalDocumentPath);

            //Place shared locks on all parents of the document.
            schemaInfo = _core.Schema.Parse(schemaInfo.LogicalParent);
            while (string.IsNullOrEmpty(schemaInfo.Name) == false)
            {
                AcquireSingleSchemaLatch(session, schemaInfo.FullLogicalPath, LatchMode.Shared);
                schemaInfo = _core.Schema.Parse(schemaInfo.LogicalParent);
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
