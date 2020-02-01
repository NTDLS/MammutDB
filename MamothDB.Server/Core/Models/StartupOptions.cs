﻿using MamothDB.Server.Core.Interfaces;
using Newtonsoft.Json;

namespace MamothDB.Server.Core.Models
{
    public class StartupOptions : IStartupOptions
    {
        public StartupOptions()
        {
        }

        public string RootPath { get; set; }

        public StartupOptions(string rootPath)
        {
            RootPath = rootPath;
        }

        [JsonIgnore]
        public string ConfigFile
        {
            get
            {
                return System.IO.Path.Join(RootPath, "mdb.config");
            }
        }
    }
}

