using MamothDB.Server.Core.Engine;
using MamothDB.Server.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MamothDB.Server.Core.Interfaces
{
    public interface IServerCore
    {
        public ServerSettings Settings { get; }
        public IOEngine IO { get; }
        public SchemaEngine Schema { get; }
        public SecurityEngine Security { get; }
    }
}
