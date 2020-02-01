using MamothDB.Server.Core.Engine;
using MamothDB.Server.Core.Interfaces;
using MamothDB.Server.Core.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;

namespace MamothDB.Server.Core
{
    public class ServerCore : IServerCore
    {
        public readonly ILogger<ServerCore> Logger;
        public ServerSettings Settings { get; private set; }
        public IOEngine IO { get; private set; }
        public SchemaEngine Schema { get; private set; }
        public SecurityEngine Security { get; private set; }

        public ServerCore(IStartupOptions startOptions, ILogger<ServerCore> logger)
        {
            Logger = logger;
            Initialize(startOptions);
        }

        private void Initialize(IStartupOptions startOptions)
        {
            Logger.LogInformation("Initializing core..");

            if (File.Exists(startOptions.ConfigFile))
            {
                Logger.LogInformation("Loading configuration.");
                //The IOManager is not initialized yet, so read the data directly.
                Settings = JsonConvert.DeserializeObject<ServerSettings>(File.ReadAllText(startOptions.ConfigFile));
            }
            else
            {
                Logger.LogInformation("Initializing new configuration.");
                Settings = new ServerSettings(startOptions.RootPath)
                {
                    //TODO: Add additional startup options.
                };
            }

            #region Create Directory Structure.

            if (Directory.Exists(Settings.RootPath) == false)
            {
                Directory.CreateDirectory(Settings.RootPath);
            }

            if (Directory.Exists(Settings.SchemaPath) == false)
            {
                Directory.CreateDirectory(Settings.SchemaPath);
                File.WriteAllText(Path.Join(Settings.SchemaPath, Constants.FileNames.SchemaCatalog), JsonConvert.SerializeObject(new MetaSchemaCollection()));
                File.WriteAllText(Path.Join(Settings.SchemaPath, Constants.FileNames.DocumentCatalog), JsonConvert.SerializeObject(new MetaDocumentCollection()));
            }

            if (Directory.Exists(Settings.TransactionPath) == false)
            {
                Directory.CreateDirectory(Settings.TransactionPath);
            }

            #endregion

            if (File.Exists(Settings.ConfigFile) == false)
            {
                //The IOManager is not initialized yet, so write the data directly.
                File.WriteAllText(Settings.ConfigFile, JsonConvert.SerializeObject(Settings));
            }

            Logger.LogInformation("Initializing security manager.");
            Security = new SecurityEngine(this);

            Logger.LogInformation("Initializing IO manager.");
            IO = new IOEngine(this);

            Logger.LogInformation("Initializing schema manager.");
            Schema = new SchemaEngine(this);
        }
    }
}
