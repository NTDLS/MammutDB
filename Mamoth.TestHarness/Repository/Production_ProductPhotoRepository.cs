using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Mamoth.Client;
using Mamoth.Common.Payload.Model;

namespace Mamoth.TestHarness.Repository
{
	public partial class Production_ProductPhotoRepository
	{        
		public void Export_Production_ProductPhoto()
		{
            using (var client = new MamothClient("https://localhost:5001", "root", "p@ssWord!"))
			{

            //if(client.Schema.Exists("AdventureWorks2008R2:Production:ProductPhoto"))
			//{
			//	return;
			//}

            client.Transaction.Enlist();

            client.Schema.CreateAll("AdventureWorks2008R2:Production:ProductPhoto");

			using (SqlConnection connection = new SqlConnection("Server=.;Database=AdventureWorks2008R2;Trusted_Connection=True;"))
			{
				connection.Open();

				try
				{
					using (SqlCommand command = new SqlCommand("SELECT * FROM Production.ProductPhoto", connection))
					{
						command.CommandTimeout = 10000;
						command.CommandType = System.Data.CommandType.Text;

						using (SqlDataReader dataReader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
						{
                            int indexOfProductPhotoID = dataReader.GetOrdinal("ProductPhotoID");
						    int indexOfThumbNailPhoto = dataReader.GetOrdinal("ThumbNailPhoto");
						    int indexOfThumbnailPhotoFileName = dataReader.GetOrdinal("ThumbnailPhotoFileName");
						    int indexOfLargePhoto = dataReader.GetOrdinal("LargePhoto");
						    int indexOfLargePhotoFileName = dataReader.GetOrdinal("LargePhotoFileName");
						    int indexOfModifiedDate = dataReader.GetOrdinal("ModifiedDate");
						    
							int rowCount = 0;


							while (dataReader.Read() && rowCount < 1000 /*easy replace*/)
							{
								if(rowCount > 0 && (rowCount % 100) == 0)
								{
									Console.WriteLine("AdventureWorks2008R2:Production:ProductPhoto: {0}", rowCount);
								}

								if(rowCount > 0 && (rowCount % 1000) == 0)
								{
									Console.WriteLine("Comitting...");
									client.Transaction.Commit();
									client.Transaction.Enlist();
								}

								try
								{
									client.Document.Create("AdventureWorks2008R2:Production:ProductPhoto", new Models.Production_ProductPhoto
									{
											ProductPhotoID= dataReader.GetInt32(indexOfProductPhotoID),
											ThumbNailPhoto= dataReader.GetNullableByteArray(indexOfThumbNailPhoto),
											ThumbnailPhotoFileName= dataReader.GetNullableString(indexOfThumbnailPhotoFileName),
											LargePhoto= dataReader.GetNullableByteArray(indexOfLargePhoto),
											LargePhotoFileName= dataReader.GetNullableString(indexOfLargePhotoFileName),
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

