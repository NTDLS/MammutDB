using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Mamoth.Client;
using Mamoth.Common.Payload.Model;

namespace Mamoth.TestHarness.Repository
{
	public partial class HumanResources_JobCandidateRepository
	{        
		public void Export_HumanResources_JobCandidate()
		{
            using (var client = new MamothClient("https://localhost:5001", "root", "p@ssWord!"))
			{

            //if(client.Schema.Exists("AdventureWorks2008R2:HumanResources:JobCandidate"))
			//{
			//	return;
			//}

            client.Transaction.Enlist();

            client.Schema.CreateAll("AdventureWorks2008R2:HumanResources:JobCandidate");

			using (SqlConnection connection = new SqlConnection("Server=.;Database=AdventureWorks2008R2;Trusted_Connection=True;"))
			{
				connection.Open();

				try
				{
					using (SqlCommand command = new SqlCommand("SELECT * FROM HumanResources.JobCandidate", connection))
					{
						command.CommandTimeout = 10000;
						command.CommandType = System.Data.CommandType.Text;

						using (SqlDataReader dataReader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
						{
                            int indexOfJobCandidateID = dataReader.GetOrdinal("JobCandidateID");
						    int indexOfBusinessEntityID = dataReader.GetOrdinal("BusinessEntityID");
						    int indexOfResume = dataReader.GetOrdinal("Resume");
						    int indexOfModifiedDate = dataReader.GetOrdinal("ModifiedDate");
						    
							int rowCount = 0;


							while (dataReader.Read() && rowCount < 1000 /*easy replace*/)
							{
								if(rowCount > 0 && (rowCount % 100) == 0)
								{
									Console.WriteLine("AdventureWorks2008R2:HumanResources:JobCandidate: {0}", rowCount);
								}

								if(rowCount > 0 && (rowCount % 1000) == 0)
								{
									Console.WriteLine("Comitting...");
									client.Transaction.Commit();
									client.Transaction.Enlist();
								}

								try
								{
									client.Document.Create("AdventureWorks2008R2:HumanResources:JobCandidate", new Models.HumanResources_JobCandidate
									{
											JobCandidateID= dataReader.GetInt32(indexOfJobCandidateID),
											BusinessEntityID= dataReader.GetNullableInt32(indexOfBusinessEntityID),
											Resume= dataReader.GetNullableString(indexOfResume),
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

