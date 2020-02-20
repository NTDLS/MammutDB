using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Mamoth.Client;
using Mamoth.Common.Payload.Model;

namespace Mamoth.TestHarness.Repository
{
	public partial class Sales_CurrencyRateRepository
	{        
		public void Export_Sales_CurrencyRate()
		{
            using (var client = new MamothClient("https://localhost:5001", "root", "p@ssWord!"))
			{

            //if(client.Schema.Exists("AdventureWorks2008R2:Sales:CurrencyRate"))
			//{
			//	return;
			//}

            client.Transaction.Enlist();

            client.Schema.CreateAll("AdventureWorks2008R2:Sales:CurrencyRate");

			using (SqlConnection connection = new SqlConnection("Server=.;Database=AdventureWorks2008R2;Trusted_Connection=True;"))
			{
				connection.Open();

				try
				{
					using (SqlCommand command = new SqlCommand("SELECT * FROM Sales.CurrencyRate", connection))
					{
						command.CommandTimeout = 10000;
						command.CommandType = System.Data.CommandType.Text;

						using (SqlDataReader dataReader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
						{
                            int indexOfCurrencyRateID = dataReader.GetOrdinal("CurrencyRateID");
						    int indexOfCurrencyRateDate = dataReader.GetOrdinal("CurrencyRateDate");
						    int indexOfFromCurrencyCode = dataReader.GetOrdinal("FromCurrencyCode");
						    int indexOfToCurrencyCode = dataReader.GetOrdinal("ToCurrencyCode");
						    int indexOfAverageRate = dataReader.GetOrdinal("AverageRate");
						    int indexOfEndOfDayRate = dataReader.GetOrdinal("EndOfDayRate");
						    int indexOfModifiedDate = dataReader.GetOrdinal("ModifiedDate");
						    
							int rowCount = 0;


							while (dataReader.Read() && rowCount < 1000 /*easy replace*/)
							{
								if(rowCount > 0 && (rowCount % 100) == 0)
								{
									Console.WriteLine("AdventureWorks2008R2:Sales:CurrencyRate: {0}", rowCount);
								}

								if(rowCount > 0 && (rowCount % 1000) == 0)
								{
									Console.WriteLine("Comitting...");
									client.Transaction.Commit();
									client.Transaction.Enlist();
								}

								try
								{
									client.Document.Create("AdventureWorks2008R2:Sales:CurrencyRate", new Models.Sales_CurrencyRate
									{
											CurrencyRateID= dataReader.GetInt32(indexOfCurrencyRateID),
											CurrencyRateDate= dataReader.GetDateTime(indexOfCurrencyRateDate),
											FromCurrencyCode= dataReader.GetString(indexOfFromCurrencyCode),
											ToCurrencyCode= dataReader.GetString(indexOfToCurrencyCode),
											AverageRate= dataReader.GetDecimal(indexOfAverageRate),
											EndOfDayRate= dataReader.GetDecimal(indexOfEndOfDayRate),
											ModifiedDate= dataReader.GetDateTime(indexOfModifiedDate),
										});
								}
								catch(Exception ex)
								{
									Console.WriteLine(ex.Message);
								}
								
								rowCount++;
							}
						}
					}
					connection.Close();
				}
				catch
				{
					//TODO: add error handling/logging
					throw;
				}

				client.Transaction.Commit();
				}
            }
		}
	}
}

