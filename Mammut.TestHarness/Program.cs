using Mammut.Client;
using System;
using System.Data.SqlClient;

namespace Mammut.TestHarness
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
            //CreateSchema();
            //DumpRootWords();
            //Exporter.ExportAll();

            (new Repository.Production_ProductRepository()).Export_Production_Product();

            Console.ReadLine();
        }

        private static void CreateSchema()
        {
            using (var client = new MammutClient("https://localhost:5001", "root", "p@ssWord!"))
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

                //var result = connection.Client.Schema.Get("AR:Sales");
                //Console.WriteLine($"Name: {result.Name}, Id: {result.Id}, Path: {result.Path}");
            }
        }

        public static void DumpRootWords()
        {
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
            {
                InitialCatalog = "WordList",
                DataSource = "localhost",
                IntegratedSecurity = true
            };

            using (var sqlConnection = new SqlConnection(sqlConnectionStringBuilder.ToString()))
            {
                sqlConnection.Open();

                using (var client = new MammutClient("https://localhost:5001", "root", "p@ssWord!"))
                {
                    client.Transaction.Enlist();

                    client.Schema.CreateAll("Word:Synonym");

                    var tSQL = "SELECT TOP 100 W.Id, W.[Text] FROM Word as W WHERE EXISTS (SELECT 1 FROM [Synonym] as S WHERE S.SourceWordId = W.Id)";
                    using (var sqlCommand = new SqlCommand(tSQL, sqlConnection))
                    {
                        using (var reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int wordId = (int)reader["Id"];
                                string wordText = reader["Text"].ToString();
                                string schema = $"Word:Synonym:{wordText}";

                                client.Schema.CreateAll(schema);

                                DumpSubWords(client, schema, wordId);

                                //System.Threading.Thread.Sleep(250);
                            }
                        }
                    }

                    client.Transaction.Commit();
                }

                sqlConnection.Close();
            }
        }

        public static void DumpSubWords(MammutClient client, string schema, int rootWordId)
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

                client.Transaction.Enlist();

                client.Schema.CreateAll(schema);

                var tSQL = $"SELECT TOP 100 W.Id, W.[Text] FROM Word as W INNER JOIN [Synonym] as S ON S.TargetWordId = W.Id WHERE S.SourceWordId = {rootWordId}";
                using (var sqlCommand = new SqlCommand(tSQL, sqlConnection))
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int wordId = (int)reader["Id"];
                            string wordText = reader["Text"].ToString();
                            string subSchema = $"{schema}:{wordText}";

                            var cust = new Customer()
                            {
                                Name = wordText,
                                EIN = $"{rand.Next(10, 99)}-{rand.Next(10000, 99999)}",
                                AnnualRevenue = rand.NextDouble() * 100000,
                                NumberOfEmployees = rand.Next(10, 1000)
                            };

                            var documentInfo = client.Document.Create(schema, cust);

                            var document = client.Document.GetById(schema, documentInfo.Id);

                            //DumpSubWords(subSchema, wordId);

                            //System.Threading.Thread.Sleep(250);
                        }

                        client.Transaction.Commit();
                    }
                }

                sqlConnection.Close();
            }
        }
    }
}
