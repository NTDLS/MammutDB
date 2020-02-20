using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Mamoth.Client;
using Mamoth.Common.Payload.Model;

namespace Mamoth.TestHarness.Repository
{
	public partial class Sales_SalesTerritoryRepository
	{        
		public void Export_Sales_SalesTerritory()
		{
            using (var client = new MamothClient("https://localhost:5001", "root", "p@ssWord!"))
			{

            //if(client.Schema.Exists("AdventureWorks2008R2:Sales:SalesTerritory"))
			//{
			//	return;
			//}

            client.Transaction.Enlist();

            client.Schema.CreateAll("AdventureWorks2008R2:Sales:SalesTerritory");

			using (SqlConnection connection = new SqlConnection("Server=.;Database=AdventureWorks2008R2;Trusted_Connection=True;"))
			{
				connection.Open();

				try
				{
					using (SqlCommand command = new SqlCommand("SELECT * FROM Sales.SalesTerritory", connection))
					{
						command.CommandTimeout = 10000;
						command.CommandType = System.Data.CommandType.Text;

						using (SqlDataReader dataReader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
						{
                            int indexOfTerritoryID = dataReader.GetOrdinal("TerritoryID");
						    int indexOfName = dataReader.GetOrdinal("Name");
						    int indexOfCountryRegionCode = dataReader.GetOrdinal("CountryRegionCode");
						    int indexOfGroup = dataReader.GetOrdinal("Group");
						    int indexOfSalesYTD = dataReader.GetOrdinal("SalesYTD");
						    int indexOfSalesLastYear = dataReader.GetOrdinal("SalesLastYear");
						    int indexOfCostYTD = dataReader.GetOrdinal("CostYTD");
						    int indexOfCostLastYear = dataReader.GetOrdinal("CostLastYear");
						    int indexOfrowguid = dataReader.GetOrdinal("rowguid");
						    int indexOfModifiedDate = dataReader.GetOrdinal("ModifiedDate");
						    
							int rowCount = 0;


							while (dataReader.Read() && rowCount < 1000 /*easy replace*/)
							{
								if(rowCount > 0 && (rowCount % 100) == 0)
								{
									Console.WriteLine("AdventureWorks2008R2:Sales:SalesTerritory: {0}", rowCount);
								}

								if(rowCount > 0 && (rowCount % 1000) == 0)
								{
									Console.WriteLine("Comitting...");
									client.Transaction.Commit();
									client.Transaction.Enlist();
								}

								try
								{
									client.Document.Create("AdventureWorks2008R2:Sales:SalesTerritory", new Models.Sales_SalesTerritory
									{
											TerritoryID= dataReader.GetInt32(indexOfTerritoryID),
											Name= dataReader.GetString(indexOfName),
											CountryRegionCode= dataReader.GetString(indexOfCountryRegionCode),
											Group= dataReader.GetString(indexOfGroup),
											SalesYTD= dataReader.GetDecimal(indexOfSalesYTD),
											SalesLastYear= dataReader.GetDecimal(indexOfSalesLastYear),
											CostYTD= dataReader.GetDecimal(indexOfCostYTD),
											CostLastYear= dataReader.GetDecimal(indexOfCostLastYear),
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

