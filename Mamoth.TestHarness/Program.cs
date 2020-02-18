using Mamoth.Client;
using System;
using System.Data.SqlClient;

namespace Mamoth.TestHarness
{
    class Customer
    {
        public string Name { get; set; }
        public string EIN { get; set; }
        public int NumberOfEmployees { get; set; }
        public double AnnualRevenue { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using(SqlCommand connection

            using (var pool = new MamothConnectionPool("https://localhost:5001", "root", "p@ssWord!"))
            {
                using (var connection = pool.GetConnection())
                {
                    /*
                    connection.Client.Transaction.Enlist();
                    connection.Client.Schema.Create("AR");
                    connection.Client.Schema.Create("AR:Sales");
                    connection.Client.Schema.Create("AR:Sales:Orders");
                    connection.Client.Schema.Create("AR:Sales:People");
                    connection.Client.Schema.Create("AR:Sales:People:Terminated");
                    connection.Client.Schema.Create("AR:Customers");
                    connection.Client.Schema.Create("AR:Customers:Prospects");
                    connection.Client.Schema.Create("AR:Customers:Contracts");
                    connection.Client.Transaction.Commit();
                    */

                    var cust = new Customer()
                    {
                        Name = "Widgets R Us",
                        EIN = "86-75309",
                        AnnualRevenue = 100.7,
                        NumberOfEmployees = 6001
                    };

                    connection.Client.Document.Create("AR:Customers", cust);

                    //var result = connection.Client.Schema.Get("AR:Sales");
                    //Console.WriteLine($"Name: {result.Name}, Id: {result.Id}, Path: {result.Path}");
                }
            }

            /*
            using (var client = new MamothClient("https://localhost:5001", "root", "p@ssWord!"))
            {
                client.Transaction.Enlist();

                client.Schema.Create("AR");
                client.Schema.Create("AR:Sales");
                client.Schema.Create("AR:Sales:Orders");
                client.Schema.Create("AR:Sales:People");
                client.Schema.Create("AR:Sales:People:Terminated");
                client.Schema.Create("AR:Customers");
                client.Schema.Create("AR:Customers:Prospects");
                client.Schema.Create("AR:Customers:Contracts");

                client.Transaction.Commit();

                //var serverVersion = client.Server.Settings.GetVersion();
                //Console.WriteLine($"{serverVersion.Name} v{serverVersion.Version}");
                client.Logout();
            }
            */
        }
    }
}
