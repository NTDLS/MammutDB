using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Mamoth.Client;
using Mamoth.Common.Payload.Model;

namespace Mamoth.TestHarness.Repository
{
	public partial class Sales_SalesOrderDetailRepository
	{        
		public void Export_Sales_SalesOrderDetail()
		{
            using (var client = new MamothClient("https://localhost:5001", "root", "p@ssWord!"))
			{

            //if(client.Schema.Exists("AdventureWorks2008R2:Sales:SalesOrderDetail"))
			//{
			//	return;
			//}

            client.Transaction.Enlist();

            client.Schema.CreateAll("AdventureWorks2008R2:Sales:SalesOrderDetail");

			using (SqlConnection connection = new SqlConnection("Server=.;Database=AdventureWorks2008R2;Trusted_Connection=True;"))
			{
				connection.Open();

				try
				{
					using (SqlCommand command = new SqlCommand("SELECT * FROM Sales.SalesOrderDetail", connection))
					{
						command.CommandTimeout = 10000;
						command.CommandType = System.Data.CommandType.Text;

						using (SqlDataReader dataReader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
						{
                            int indexOfSalesOrderID = dataReader.GetOrdinal("SalesOrderID");
						    int indexOfSalesOrderDetailID = dataReader.GetOrdinal("SalesOrderDetailID");
						    int indexOfCarrierTrackingNumber = dataReader.GetOrdinal("CarrierTrackingNumber");
						    int indexOfOrderQty = dataReader.GetOrdinal("OrderQty");
						    int indexOfProductID = dataReader.GetOrdinal("ProductID");
						    int indexOfSpecialOfferID = dataReader.GetOrdinal("SpecialOfferID");
						    int indexOfUnitPrice = dataReader.GetOrdinal("UnitPrice");
						    int indexOfUnitPriceDiscount = dataReader.GetOrdinal("UnitPriceDiscount");
						    int indexOfLineTotal = dataReader.GetOrdinal("LineTotal");
						    int indexOfrowguid = dataReader.GetOrdinal("rowguid");
						    int indexOfModifiedDate = dataReader.GetOrdinal("ModifiedDate");
						    
							int rowCount = 0;


							while (dataReader.Read() && rowCount < 1000 /*easy replace*/)
							{
								if(rowCount > 0 && (rowCount % 100) == 0)
								{
									Console.WriteLine("AdventureWorks2008R2:Sales:SalesOrderDetail: {0}", rowCount);
								}

								if(rowCount > 0 && (rowCount % 1000) == 0)
								{
									Console.WriteLine("Comitting...");
									client.Transaction.Commit();
									client.Transaction.Enlist();
								}

								try
								{
									client.Document.Create("AdventureWorks2008R2:Sales:SalesOrderDetail", new Models.Sales_SalesOrderDetail
									{
											SalesOrderID= dataReader.GetInt32(indexOfSalesOrderID),
											SalesOrderDetailID= dataReader.GetInt32(indexOfSalesOrderDetailID),
											CarrierTrackingNumber= dataReader.GetNullableString(indexOfCarrierTrackingNumber),
											OrderQty= dataReader.GetInt16(indexOfOrderQty),
											ProductID= dataReader.GetInt32(indexOfProductID),
											SpecialOfferID= dataReader.GetInt32(indexOfSpecialOfferID),
											UnitPrice= dataReader.GetDecimal(indexOfUnitPrice),
											UnitPriceDiscount= dataReader.GetDecimal(indexOfUnitPriceDiscount),
											LineTotal= dataReader.GetDecimal(indexOfLineTotal),
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

