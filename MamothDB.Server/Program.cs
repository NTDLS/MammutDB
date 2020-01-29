using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MamothDB.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            bool requirementsMet = false;

            if (args.Count() > 1)
            {
                foreach (var arg in args)
                {
                    if (arg.ToLower() == "--root")
                    {
                        requirementsMet = true;
                        break;
                    }
                }
            }

            if (requirementsMet == false)
            {
                ShowCommandlineHelp();
                return;
            }

            CreateHostBuilder(args).Build().Run();
        }

        private static void ShowCommandlineHelp()
        {
            Console.WriteLine("MamothDB.Server.exe --root <root_path>");
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            var builder = Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())

                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

                return builder;
        }
    }
}
