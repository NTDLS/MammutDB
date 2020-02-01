using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MamothDB.Server.Core.Interfaces
{
    public interface IStartupOptions
    {
        public string RootPath { get; set; }
        public string ConfigFile { get; }
    }
}
