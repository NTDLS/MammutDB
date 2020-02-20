using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Mammut.Client;
using Mammut.Common.Payload.Model;

namespace Mammut.TestHarness.Repository
{
	public partial class Sales_SalesPersonRepository
	{        
		public void Export_Sales_SalesPerson()
		{
            using (var client = new MammutClient("https://localhost:5001", "root", "p@ssWord!"))
			{

            //if(client.Schema.Exists("AdventureWorks2008R2:Sales:SalesPerson"))
			//{
			//	return;
			//}

            client.Transaction.Enlist();

            client.Schema.CreateAll("AdventureWorks2008R2:Sales:SalesPerson");

			using (SqlConnection connection = new SqlConnection("Server=.;Database=AdventureWorks2008R2;Trusted_Connection=True;"))
			{
				connection.Open();

				try
				{
					using (SqlCommand command = new SqlCommand("SELECT * FROM Sales.SalesPerson", connection))
					{
						command.CommandTimeout = 10000;
						command.CommandType = System.Data.CommandType.Text;

						using (SqlDataReader dataReader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
						{
                            int indexOfBusinessEntityID = dataReader.GetOrdinal("BusinessEntityID");
						    int indexOfTerritoryID = dataReader.GetOrdinal("TerritoryID");
						    int indexOfSalesQuota = dataReader.GetOrdinal("SalesQuota");
						    int indexOfBonus = dataReader.GetOrdinal("Bonus");
						    int indexOfCommissionPct = dataReader.GetOrdinal("CommissionPct");
						    int indexOfSalesYTD = dataReader.GetOrdinal("SalesYTD");
						    int indexOfSalesLastYear = dataReader.GetOrdinal("SalesLastYear");
						    int indexOfrowguid = dataReader.GetOrdinal("rowguid");
						    int indexOfModifiedDate = dataReader.GetOrdinal("ModifiedDate");
						    
							int rowCount = 0;


							while (dataReader.Read() && rowCount < 1000 /*easy replace*/)
							{
								if(rowCount > 0 && (rowCount % 100) == 0)
								{
									Console.WriteLine("AdventureWorks2008R2:Sales:SalesPerson: {0}", rowCount);
								}

								if(rowCount > 0 && (rowCount % 1000) == 0)
								{
									Console.WriteLine("Comitting...");
									client.Transaction.Commit();
									client.Transaction.Enlist();
								}

								try
								{
									client.Document.Create("AdventureWorks2008R2:Sales:SalesPerson", new Models.Sales_SalesPerson
									{
											BusinessEntityID= dataReader.GetInt32(indexOfBusinessEntityID),
											TerritoryID= dataReader.GetNullableInt32(indexOfTerritoryID),
											SalesQuota= dataReader.GetNullableDecimal(indexOfSalesQuota),
											Bonus= dataReader.GetDecimal(indexOfBonus),
											CommissionPct= dataReader.GetDecimal(indexOfCommissionPct),
											SalesYTD= dataReader.GetDecimal(indexOfSalesYTD),
											SalesLastYear= dataReader.GetDecimal(indexOfSalesLastYear),
											rowguid= dataReader.GetGuid(indexOfrowguid),
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

