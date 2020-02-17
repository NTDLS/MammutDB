using MamothDB.Server.Core.Engine;
using MamothDB.Server.Core.Interfaces;
using MamothDB.Server.Core.Models.Persist;
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
        public SessionEngine Session { get; private set; }
        public TransactionEngine Transaction { get; private set; }
        public LatchEngine Latch { get; private set; }

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

            if (Directory.Exists(Settings.TransactionPath) == false)
            {
                Directory.CreateDirectory(Settings.TransactionPath);
            }

            #endregion

            //Settings.LoginFile

            if (File.Exists(Settings.ConfigFile) == false)
            {
                //The IOManager is not initialized yet, so write the data directly.
                File.WriteAllText(Settings.ConfigFile, JsonConvert.SerializeObject(Settings));
            }

            if (File.Exists(Settings.LoginFile) == false)
            {
                var loginCatalog = new MetaLoginCollection();

#if DEBUG
                var defaultLogin = new MetaLogin("root");
                defaultLogin.SetPassword("p@ssWord!");
                loginCatalog.Add(defaultLogin);
#endif

                //The IOManager is not initialized yet, so write the data directly.
                File.WriteAllText(Settings.LoginFile, JsonConvert.SerializeObject(loginCatalog));
            }

            Logger.LogInformation("Initializing security manager.");
            Security = new SecurityEngine(this);

            Logger.LogInformation("Initializing IO manager.");
            IO = new IOEngine(this);

            Logger.LogInformation("Initializing schema manager.");
            Schema = new SchemaEngine(this);

            Logger.LogInformation("Initializing session manager.");
            Session = new SessionEngine(this);

            Logger.LogInformation("Initializing transaction manager.");
            Transaction = new TransactionEngine(this);

            Logger.LogInformation("Initializing latch manager.");
            Latch = new LatchEngine(this);
        }
    }
}
