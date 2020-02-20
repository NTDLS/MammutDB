using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Mammut.Client;
using Mammut.Common.Payload.Model;

namespace Mammut.TestHarness.Repository
{
	public partial class Purchasing_VendorRepository
	{        
		public void Export_Purchasing_Vendor()
		{
            using (var client = new MammutClient("https://localhost:5001", "root", "p@ssWord!"))
			{

            //if(client.Schema.Exists("AdventureWorks2008R2:Purchasing:Vendor"))
			//{
			//	return;
			//}

            client.Transaction.Enlist();

            client.Schema.CreateAll("AdventureWorks2008R2:Purchasing:Vendor");

			using (SqlConnection connection = new SqlConnection("Server=.;Database=AdventureWorks2008R2;Trusted_Connection=True;"))
			{
				connection.Open();

				try
				{
					using (SqlCommand command = new SqlCommand("SELECT * FROM Purchasing.Vendor", connection))
					{
						command.CommandTimeout = 10000;
						command.CommandType = System.Data.CommandType.Text;

						using (SqlDataReader dataReader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
						{
                            int indexOfBusinessEntityID = dataReader.GetOrdinal("BusinessEntityID");
						    int indexOfAccountNumber = dataReader.GetOrdinal("AccountNumber");
						    int indexOfName = dataReader.GetOrdinal("Name");
						    int indexOfCreditRating = dataReader.GetOrdinal("CreditRating");
						    int indexOfPreferredVendorStatus = dataReader.GetOrdinal("PreferredVendorStatus");
						    int indexOfActiveFlag = dataReader.GetOrdinal("ActiveFlag");
						    int indexOfPurchasingWebServiceURL = dataReader.GetOrdinal("PurchasingWebServiceURL");
						    int indexOfModifiedDate = dataReader.GetOrdinal("ModifiedDate");
						    
							int rowCount = 0;


							while (dataReader.Read() && rowCount < 1000 /*easy replace*/)
							{
								if(rowCount > 0 && (rowCount % 100) == 0)
								{
									Console.WriteLine("AdventureWorks2008R2:Purchasing:Vendor: {0}", rowCount);
								}

								if(rowCount > 0 && (rowCount % 1000) == 0)
								{
									Console.WriteLine("Comitting...");
									client.Transaction.Commit();
									client.Transaction.Enlist();
								}

								try
								{
									client.Document.Create("AdventureWorks2008R2:Purchasing:Vendor", new Models.Purchasing_Vendor
									{
											BusinessEntityID= dataReader.GetInt32(indexOfBusinessEntityID),
											AccountNumber= dataReader.GetString(indexOfAccountNumber),
											Name= dataReader.GetString(indexOfName),
											CreditRating= dataReader.GetByte(indexOfCreditRating),
											PreferredVendorStatus= dataReader.GetBoolean(indexOfPreferredVendorStatus),
											ActiveFlag= dataReader.GetBoolean(indexOfActiveFlag),
											PurchasingWebServiceURL= dataReader.GetNullableString(indexOfPurchasingWebServiceURL),
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

