using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using MamothDB.Server.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MamothDB.Server.Core.Models
{
    public class ServerCoreSettings : IServerCoreSettings
    {
        public ServerCoreSettings()
        {
        }

        public ServerCoreSettings(string rootPath)
        {
            RootPath = rootPath;
        }

        public string RootPath { get; }

        public string ConfigFile
        {
            get
            {
                return System.IO.Path.Join(RootPath, "mdb.config");
            }
        }

        public string UndoPath
        {
            get
            {
                return System.IO.Path.Join(RootPath, "undo");
            }
        }

        public string DataPath
        {
            get
            {
                return System.IO.Path.Join(RootPath, "data");
            }
        }
    }
}

