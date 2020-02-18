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
        public ILogger<ServerCore> Logger;
        public ServerSettings Settings { get; private set; }
        public IOEngine IO { get; private set; }
        public SchemaEngine Schema { get; private set; }
        public SecurityEngine Security { get; private set; }
        public SessionEngine Session { get; private set; }
        public TransactionEngine Transaction { get; private set; }
        public LatchEngine Latch { get; private set; }
        public DocumentEngine Document { get; private set; }

        public ServerCore(IStartupOptions startOptions, ILogger<ServerCore> logger)
        {
            Logger = logger;
            Initialize(startOptions);
        }

        public void LogInformation(string text) => Logger.LogInformation(text);
        public void LogDebug(string text) => Logger.LogDebug(text);
        public void LogError(string text) => Logger.LogError(text);
        public void LogWarning(string text) => Logger.LogWarning(text);

        private void Initialize(IStartupOptions startOptions)
        {
            LogInformation("Initializing core..");

            if (File.Exists(startOptions.ConfigFile))
            {
                LogInformation("Loading configuration.");
                //The IOManager is not initialized yet, so read the data directly.
                Settings = JsonConvert.DeserializeObject<ServerSettings>(File.ReadAllText(startOptions.ConfigFile));
            }
            else
            {
                LogInformation("Initializing new configuration.");
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

            LogInformation("Initializing security engine.");
            Security = new SecurityEngine(this);

            LogInformation("Initializing IO engine.");
            IO = new IOEngine(this);

            LogInformation("Initializing schema engine.");
            Schema = new SchemaEngine(this);

            LogInformation("Initializing session engine.");
            Session = new SessionEngine(this);

            LogInformation("Initializing latch engine.");
            Latch = new LatchEngine(this);

            LogInformation("Initializing transaction engine.");
            Transaction = new TransactionEngine(this);

            LogInformation("Initializing document engine.");
            Document = new DocumentEngine(this);
            

            LogInformation("Starting transaction recovery.");
            Transaction.Recover();
            LogInformation("Transaction recovery complete.");
        }
    }
}
