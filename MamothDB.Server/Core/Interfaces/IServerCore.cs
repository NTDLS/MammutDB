using MamothDB.Server.Core.Engine;
using MamothDB.Server.Core.Models.Persist;

namespace MamothDB.Server.Core.Interfaces
{
    public interface IServerCore
    {
        public ServerSettings Settings { get; }
        public IOEngine IO { get; }
        public SchemaEngine Schema { get; }
        public SecurityEngine Security { get; }
        public SessionEngine Session { get; }
    }
}
