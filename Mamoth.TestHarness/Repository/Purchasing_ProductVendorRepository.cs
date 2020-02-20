using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Mamoth.Client;
using Mamoth.Common.Payload.Model;

namespace Mamoth.TestHarness.Repository
{
	public partial class Purchasing_ProductVendorRepository
	{        
		public void Export_Purchasing_ProductVendor()
		{
            using (var client = new MamothClient("https://localhost:5001", "root", "p@ssWord!"))
			{

            //if(client.Schema.Exists("AdventureWorks2008R2:Purchasing:ProductVendor"))
			//{
			//	return;
			//}

            client.Transaction.Enlist();

            client.Schema.CreateAll("AdventureWorks2008R2:Purchasing:ProductVendor");

			using (SqlConnection connection = new SqlConnection("Server=.;Database=AdventureWorks2008R2;Trusted_Connection=True;"))
			{
				connection.Open();

				try
				{
					using (SqlCommand command = new SqlCommand("SELECT * FROM Purchasing.ProductVendor", connection))
					{
						command.CommandTimeout = 10000;
						command.CommandType = System.Data.CommandType.Text;

						using (SqlDataReader dataReader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
						{
                            int indexOfProductID = dataReader.GetOrdinal("ProductID");
						    int indexOfBusinessEntityID = dataReader.GetOrdinal("BusinessEntityID");
						    int indexOfAverageLeadTime = dataReader.GetOrdinal("AverageLeadTime");
						    int indexOfStandardPrice = dataReader.GetOrdinal("StandardPrice");
						    int indexOfLastReceiptCost = dataReader.GetOrdinal("LastReceiptCost");
						    int indexOfLastReceiptDate = dataReader.GetOrdinal("LastReceiptDate");
						    int indexOfMinOrderQty = dataReader.GetOrdinal("MinOrderQty");
						    int indexOfMaxOrderQty = dataReader.GetOrdinal("MaxOrderQty");
						    int indexOfOnOrderQty = dataReader.GetOrdinal("OnOrderQty");
						    int indexOfUnitMeasureCode = dataReader.GetOrdinal("UnitMeasureCode");
						    int indexOfModifiedDate = dataReader.GetOrdinal("ModifiedDate");
						    
							int rowCount = 0;


							while (dataReader.Read() && rowCount < 1000 /*easy replace*/)
							{
								if(rowCount > 0 && (rowCount % 100) == 0)
								{
									Console.WriteLine("AdventureWorks2008R2:Purchasing:ProductVendor: {0}", rowCount);
								}

								if(rowCount > 0 && (rowCount % 1000) == 0)
								{
									Console.WriteLine("Comitting...");
									client.Transaction.Commit();
									client.Transaction.Enlist();
								}

								try
								{
									client.Document.Create("AdventureWorks2008R2:Purchasing:ProductVendor", new Models.Purchasing_ProductVendor
									{
											ProductID= dataReader.GetInt32(indexOfProductID),
											BusinessEntityID= dataReader.GetInt32(indexOfBusinessEntityID),
											AverageLeadTime= dataReader.GetInt32(indexOfAverageLeadTime),
											StandardPrice= dataReader.GetDecimal(indexOfStandardPrice),
											LastReceiptCost= dataReader.GetNullableDecimal(indexOfLastReceiptCost),
											LastReceiptDate= dataReader.GetNullableDateTime(indexOfLastReceiptDate),
											MinOrderQty= dataReader.GetInt32(indexOfMinOrderQty),
											MaxOrderQty= dataReader.GetInt32(indexOfMaxOrderQty),
											OnOrderQty= dataReader.GetNullableInt32(indexOfOnOrderQty),
											UnitMeasureCode= dataReader.GetString(indexOfUnitMeasureCode),
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

