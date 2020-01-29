using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MamothDB.Server.Core.Interfaces
{
    public interface IServerCore
    {
        public IServerCoreSettings Settings { get; }
    }
}
