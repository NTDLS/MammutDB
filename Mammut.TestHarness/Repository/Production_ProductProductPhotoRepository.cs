using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Mammut.Client;
using Mammut.Common.Payload.Model;

namespace Mammut.TestHarness.Repository
{
	public partial class Production_ProductProductPhotoRepository
	{        
		public void Export_Production_ProductProductPhoto()
		{
            using (var client = new MammutClient("https://localhost:5001", "root", "p@ssWord!"))
			{

            //if(client.Schema.Exists("AdventureWorks2008R2:Production:ProductProductPhoto"))
			//{
			//	return;
			//}

            client.Transaction.Enlist();

            client.Schema.CreateAll("AdventureWorks2008R2:Production:ProductProductPhoto");

			using (SqlConnection connection = new SqlConnection("Server=.;Database=AdventureWorks2008R2;Trusted_Connection=True;"))
			{
				connection.Open();

				try
				{
					using (SqlCommand command = new SqlCommand("SELECT * FROM Production.ProductProductPhoto", connection))
					{
						command.CommandTimeout = 10000;
						command.CommandType = System.Data.CommandType.Text;

						using (SqlDataReader dataReader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
						{
                            int indexOfProductID = dataReader.GetOrdinal("ProductID");
						    int indexOfProductPhotoID = dataReader.GetOrdinal("ProductPhotoID");
						    int indexOfPrimary = dataReader.GetOrdinal("Primary");
						    int indexOfModifiedDate = dataReader.GetOrdinal("ModifiedDate");
						    
							int rowCount = 0;


							while (dataReader.Read() && rowCount < 1000 /*easy replace*/)
							{
								if(rowCount > 0 && (rowCount % 100) == 0)
								{
									Console.WriteLine("AdventureWorks2008R2:Production:ProductProductPhoto: {0}", rowCount);
								}

								if(rowCount > 0 && (rowCount % 1000) == 0)
								{
									Console.WriteLine("Comitting...");
									client.Transaction.Commit();
									client.Transaction.Enlist();
								}

								try
								{
									client.Document.Create("AdventureWorks2008R2:Production:ProductProductPhoto", new Models.Production_ProductProductPhoto
									{
											ProductID= dataReader.GetInt32(indexOfProductID),
											ProductPhotoID= dataReader.GetInt32(indexOfProductPhotoID),
											Primary= dataReader.GetBoolean(indexOfPrimary),
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

