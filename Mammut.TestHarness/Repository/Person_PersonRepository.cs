using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Mammut.Client;
using Mammut.Common.Payload.Model;

namespace Mammut.TestHarness.Repository
{
	public partial class Person_PersonRepository
	{        
		public void Export_Person_Person()
		{
            using (var client = new MammutClient("https://localhost:5001", "root", "p@ssWord!"))
			{

            //if(client.Schema.Exists("AdventureWorks2008R2:Person:Person"))
			//{
			//	return;
			//}

            client.Transaction.Enlist();

            client.Schema.CreateAll("AdventureWorks2008R2:Person:Person");

			using (SqlConnection connection = new SqlConnection("Server=.;Database=AdventureWorks2008R2;Trusted_Connection=True;"))
			{
				connection.Open();

				try
				{
					using (SqlCommand command = new SqlCommand("SELECT * FROM Person.Person", connection))
					{
						command.CommandTimeout = 10000;
						command.CommandType = System.Data.CommandType.Text;

						using (SqlDataReader dataReader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
						{
                            int indexOfBusinessEntityID = dataReader.GetOrdinal("BusinessEntityID");
						    int indexOfPersonType = dataReader.GetOrdinal("PersonType");
						    int indexOfNameStyle = dataReader.GetOrdinal("NameStyle");
						    int indexOfTitle = dataReader.GetOrdinal("Title");
						    int indexOfFirstName = dataReader.GetOrdinal("FirstName");
						    int indexOfMiddleName = dataReader.GetOrdinal("MiddleName");
						    int indexOfLastName = dataReader.GetOrdinal("LastName");
						    int indexOfSuffix = dataReader.GetOrdinal("Suffix");
						    int indexOfEmailPromotion = dataReader.GetOrdinal("EmailPromotion");
						    int indexOfAdditionalContactInfo = dataReader.GetOrdinal("AdditionalContactInfo");
						    int indexOfDemographics = dataReader.GetOrdinal("Demographics");
						    int indexOfrowguid = dataReader.GetOrdinal("rowguid");
						    int indexOfModifiedDate = dataReader.GetOrdinal("ModifiedDate");
						    
							int rowCount = 0;


							while (dataReader.Read() && rowCount < 1000 /*easy replace*/)
							{
								if(rowCount > 0 && (rowCount % 100) == 0)
								{
									Console.WriteLine("AdventureWorks2008R2:Person:Person: {0}", rowCount);
								}

								if(rowCount > 0 && (rowCount % 1000) == 0)
								{
									Console.WriteLine("Comitting...");
									client.Transaction.Commit();
									client.Transaction.Enlist();
								}

								try
								{
									client.Document.Create("AdventureWorks2008R2:Person:Person", new Models.Person_Person
									{
											BusinessEntityID= dataReader.GetInt32(indexOfBusinessEntityID),
											PersonType= dataReader.GetString(indexOfPersonType),
											NameStyle= dataReader.GetBoolean(indexOfNameStyle),
											Title= dataReader.GetNullableString(indexOfTitle),
											FirstName= dataReader.GetString(indexOfFirstName),
											MiddleName= dataReader.GetNullableString(indexOfMiddleName),
											LastName= dataReader.GetString(indexOfLastName),
											Suffix= dataReader.GetNullableString(indexOfSuffix),
											EmailPromotion= dataReader.GetInt32(indexOfEmailPromotion),
											AdditionalContactInfo= dataReader.GetNullableString(indexOfAdditionalContactInfo),
											Demographics= dataReader.GetNullableString(indexOfDemographics),
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

