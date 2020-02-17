using MamothDB.Server.Core.Engine;
using MamothDB.Server.Core.Models.Persist;
using Microsoft.Extensions.Logging;

namespace MamothDB.Server.Core.Interfaces
{
    public interface IServerCore
    {
        public ServerSettings Settings { get; }
        public IOEngine IO { get; }
        public SchemaEngine Schema { get; }
        public SecurityEngine Security { get; }
        public SessionEngine Session { get; }
        public TransactionEngine Transaction { get; }
        public LatchEngine Latch { get; }
    }
}
