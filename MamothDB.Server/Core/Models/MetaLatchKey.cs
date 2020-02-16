using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MamothDB.Server.Core.Constants;

namespace MamothDB.Server.Core.Models
{
    /// <summary>
    /// Represents a key to a latch.
    /// </summary>
    public class MetaLatchKey
    {
        public MetaLatch Latch { get; private set; }
        public LatchMode Mode { get; private set; }
        public MetaTransaction Transaction { get; private set; }

        public MetaLatchKey(MetaLatch latch, MetaTransaction transaction, LatchMode mode)
        {
            Latch = latch;
            Transaction = transaction;
            Mode = mode;
        }
    }
}
