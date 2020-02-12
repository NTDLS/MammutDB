using Mamoth.Client;
using System;

namespace Mamoth.TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var pool = new MamothConnectionPool("https://localhost:5001", "root", ""))
            {
                using (var connection = pool.GetConnection())
                {
                    connection.Client.Schema.Create("AR");
                    connection.Client.Schema.Create("AR:Sales");
                    connection.Client.Schema.Create("AR:Sales:Orders");
                    connection.Client.Schema.Create("AR:Sales:People");
                    connection.Client.Schema.Create("AR:Sales:People:Terminated");
                    connection.Client.Schema.Create("AR:Customers");
                    connection.Client.Schema.Create("AR:Customers:Prospects");
                    connection.Client.Schema.Create("AR:Customers:Contracts");
                }
            }

            /*
            using (var client = new MamothClient("https://localhost:5001", "root", ""))
            {
                client.Schema.Create("AR");
                client.Schema.Create("AR:Sales");
                client.Schema.Create("AR:Sales:Orders");
                client.Schema.Create("AR:Sales:People");
                client.Schema.Create("AR:Sales:People:Terminated");
                client.Schema.Create("AR:Customers");
                client.Schema.Create("AR:Customers:Prospects");
                client.Schema.Create("AR:Customers:Contracts");

                //var serverVersion = client.Server.Settings.GetVersion();
                //Console.WriteLine($"{serverVersion.Name} v{serverVersion.Version}");
                client.Logout();
            }
            */

        }
    }
}
