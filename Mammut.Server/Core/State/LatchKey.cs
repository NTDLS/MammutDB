using static Mammut.Server.Core.Constants;

namespace Mammut.Server.Core.State
{
    /// <summary>
    /// Represents a key to a latch.
    /// </summary>
    public class LatchKey
    {
        public Latch Latch { get; private set; }
        public LatchMode Mode { get; private set; }
        public Transaction Transaction { get; private set; }

        public LatchKey(Latch latch, Transaction transaction, LatchMode mode)
        {
            Latch = latch;
            Transaction = transaction;
            Mode = mode;
        }
    }
}
