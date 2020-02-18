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
            //CreateSchemaWithPool();
            //CreateSchemaWithSingleConnection();
            DumpWordList();
        }

        private static void CreateSchemaWithPool()
        {
            using (var pool = new MamothConnectionPool("https://localhost:5001", "root", "p@ssWord!"))
            {
                using (var connection = pool.GetConnection())
                {
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

                    //var result = connection.Client.Schema.Get("AR:Sales");
                    //Console.WriteLine($"Name: {result.Name}, Id: {result.Id}, Path: {result.Path}");
                }
            }
        }

        private static void CreateSchemaWithSingleConnection()
        {
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
        }

        public static void DumpWordList()
        {
            var rand = new Random();

            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
            {
                InitialCatalog = "WordList",
                DataSource = "localhost",
                IntegratedSecurity = true
            };

            using (var sqlConnection = new SqlConnection(sqlConnectionStringBuilder.ToString()))
            {
                sqlConnection.Open();

                using (var pool = new MamothConnectionPool("https://localhost:5001", "root", "p@ssWord!"))
                {
                    using (var connection = pool.GetConnection())
                    {
                        connection.Client.Transaction.Enlist();

                        using (var sqlCommand = new SqlCommand("SELECT [Text] FROM Word", sqlConnection))
                        {
                            using (var reader = sqlCommand.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var cust = new Customer()
                                    {
                                        Name = reader["Text"].ToString(),
                                        EIN = $"{rand.Next(10, 99)}-{rand.Next(10000, 99999)}",
                                        AnnualRevenue = rand.NextDouble() * 100000,
                                        NumberOfEmployees = rand.Next(10, 1000)
                                    };

                                    connection.Client.Document.Create("AR:Customers", cust);
                                }
                            }
                        }

                        connection.Client.Transaction.Commit();
                    }
                }

                sqlConnection.Close();
            }
        }
    }
}
