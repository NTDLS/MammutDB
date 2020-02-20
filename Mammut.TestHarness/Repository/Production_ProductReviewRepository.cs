using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Mammut.Client;
using Mammut.Common.Payload.Model;

namespace Mammut.TestHarness.Repository
{
	public partial class Production_ProductReviewRepository
	{        
		public void Export_Production_ProductReview()
		{
            using (var client = new MammutClient("https://localhost:5001", "root", "p@ssWord!"))
			{

            //if(client.Schema.Exists("AdventureWorks2008R2:Production:ProductReview"))
			//{
			//	return;
			//}

            client.Transaction.Enlist();

            client.Schema.CreateAll("AdventureWorks2008R2:Production:ProductReview");

			using (SqlConnection connection = new SqlConnection("Server=.;Database=AdventureWorks2008R2;Trusted_Connection=True;"))
			{
				connection.Open();

				try
				{
					using (SqlCommand command = new SqlCommand("SELECT * FROM Production.ProductReview", connection))
					{
						command.CommandTimeout = 10000;
						command.CommandType = System.Data.CommandType.Text;

						using (SqlDataReader dataReader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
						{
                            int indexOfProductReviewID = dataReader.GetOrdinal("ProductReviewID");
						    int indexOfProductID = dataReader.GetOrdinal("ProductID");
						    int indexOfReviewerName = dataReader.GetOrdinal("ReviewerName");
						    int indexOfReviewDate = dataReader.GetOrdinal("ReviewDate");
						    int indexOfEmailAddress = dataReader.GetOrdinal("EmailAddress");
						    int indexOfRating = dataReader.GetOrdinal("Rating");
						    int indexOfComments = dataReader.GetOrdinal("Comments");
						    int indexOfModifiedDate = dataReader.GetOrdinal("ModifiedDate");
						    
							int rowCount = 0;


							while (dataReader.Read() && rowCount < 1000 /*easy replace*/)
							{
								if(rowCount > 0 && (rowCount % 100) == 0)
								{
									Console.WriteLine("AdventureWorks2008R2:Production:ProductReview: {0}", rowCount);
								}

								if(rowCount > 0 && (rowCount % 1000) == 0)
								{
									Console.WriteLine("Comitting...");
									client.Transaction.Commit();
									client.Transaction.Enlist();
								}

								try
								{
									client.Document.Create("AdventureWorks2008R2:Production:ProductReview", new Models.Production_ProductReview
									{
											ProductReviewID= dataReader.GetInt32(indexOfProductReviewID),
											ProductID= dataReader.GetInt32(indexOfProductID),
											ReviewerName= dataReader.GetString(indexOfReviewerName),
											ReviewDate= dataReader.GetDateTime(indexOfReviewDate),
											EmailAddress= dataReader.GetString(indexOfEmailAddress),
											Rating= dataReader.GetInt32(indexOfRating),
											Comments= dataReader.GetNullableString(indexOfComments),
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

