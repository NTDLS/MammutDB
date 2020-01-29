using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MamothDB.Server.Core.Interfaces
{
    public interface IServerCoreSettings
    {
        public string RootPath { get; }
        public string ConfigFile { get; }
        public string UndoPath { get; }
        public string DataPath { get; }
    }
}
