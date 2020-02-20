using Mammut.Server.Core.Engine;
using Mammut.Server.Core.Models.Persist;
using Microsoft.Extensions.Logging;

namespace Mammut.Server.Core.Interfaces
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
        public DocumentEngine Document { get; }
    }
}
