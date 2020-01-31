using Mamoth.Client;
using System;

namespace Mamoth.TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new MamothClient("https://localhost:5001", "root", ""))
            {
                //var serverVersion = client.Server.Settings.GetVersion();
                //Console.WriteLine($"{serverVersion.Name} v{serverVersion.Version}");
                client.Logout();
            }
        }
    }
}
