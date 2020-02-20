using Newtonsoft.Json;

namespace Mammut.Server.Core.Models.Persist
{
    public class MetaServerSettings
    {
        public string RootPath { get; set; } = string.Empty;
        public long MaxCacheSize { get; set; } = 1024 * 1024;

        #region ~CTor.

        public MetaServerSettings()
        {
        }

        public MetaServerSettings(string rootPath)
        {
            RootPath = rootPath;
        }

        #endregion

        [JsonIgnore]
        public string ConfigFile
        {
            get
            {
                return System.IO.Path.Join(RootPath, Constants.Filesystem.ServerSettings);
            }
        }

        [JsonIgnore]
        public string LoginFile
        {
            get
            {
                return System.IO.Path.Join(RootPath, Constants.Filesystem.LoginCatalog);
            }
        }
        

        [JsonIgnore]
        public string TransactionPath
        {
            get
            {
                return System.IO.Path.Join(RootPath, Constants.Filesystem.TransactionDirectory);
            }
        }

        [JsonIgnore]
        public string SchemaPath
        {
            get
            {
                return System.IO.Path.Join(RootPath, Constants.Filesystem.SchemaDirectory);
            }
        }
    }
}

