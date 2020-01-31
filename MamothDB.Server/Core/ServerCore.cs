using Autofac.Core;
using Autofac.Core.Registration;
using MamothDB.Server.Core.Engine;
using MamothDB.Server.Core.Interfaces;
using MamothDB.Server.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MamothDB.Server.Core
{
    public class ServerCore: IServerCore
    {
        public IServerCoreSettings Settings { get; }
        public Security Security { get; }

        public ServerCore(IServerCoreSettings settings)
        {
            Settings = settings;

            Security = new Security(this);
        }
    }
}
