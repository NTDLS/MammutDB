using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MamothDB.Server.Core.Models
{
    public class MetaTransaction
    {
        /// <summary>
        /// Implicit transactions were created by the engine and must be completed at the end of any action.
        /// </summary>
        public bool IsImplicit { get; private set; }
        public MetaSession Session { get; private set; }

        public MetaTransaction(MetaSession session, bool isImplicit)
        {
            IsImplicit = isImplicit;
            Session = session;
        }

        public void Commit()
        {
            Session.CurrentTransaction = null;
        }

        public void Rollback()
        {
            Session.CurrentTransaction = null;
        }
    }
}
