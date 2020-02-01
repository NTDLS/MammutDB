using MamothDB.Server.Core.Interfaces;
using Newtonsoft.Json;

namespace MamothDB.Server.Core.Models
{
    public class ServerSettings
    {
        public string RootPath { get; set; } = string.Empty;
        public long MaxCacheSize { get; set; } = 1024 * 1024;

        #region ~CTor.

        public ServerSettings()
        {
        }

        public ServerSettings(string rootPath)
        {
            RootPath = rootPath;
        }

        #endregion

        [JsonIgnore]
        public string ConfigFile
        {
            get
            {
                return System.IO.Path.Join(RootPath, "_server.json");
            }
        }

        [JsonIgnore]
        public string TransactionPath
        {
            get
            {
                return System.IO.Path.Join(RootPath, "_transaction");
            }
        }

        [JsonIgnore]
        public string SchemaPath
        {
            get
            {
                return System.IO.Path.Join(RootPath, "_schema");
            }
        }
    }
}

