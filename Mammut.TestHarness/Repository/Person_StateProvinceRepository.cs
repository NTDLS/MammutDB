using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Mammut.Client;
using Mammut.Common.Payload.Model;

namespace Mammut.TestHarness.Repository
{
	public partial class Person_StateProvinceRepository
	{        
		public void Export_Person_StateProvince()
		{
            using (var client = new MammutClient("https://localhost:5001", "root", "p@ssWord!"))
			{

            //if(client.Schema.Exists("AdventureWorks2008R2:Person:StateProvince"))
			//{
			//	return;
			//}

            client.Transaction.Enlist();

            client.Schema.CreateAll("AdventureWorks2008R2:Person:StateProvince");

			using (SqlConnection connection = new SqlConnection("Server=.;Database=AdventureWorks2008R2;Trusted_Connection=True;"))
			{
				connection.Open();

				try
				{
					using (SqlCommand command = new SqlCommand("SELECT * FROM Person.StateProvince", connection))
					{
						command.CommandTimeout = 10000;
						command.CommandType = System.Data.CommandType.Text;

						using (SqlDataReader dataReader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
						{
                            int indexOfStateProvinceID = dataReader.GetOrdinal("StateProvinceID");
						    int indexOfStateProvinceCode = dataReader.GetOrdinal("StateProvinceCode");
						    int indexOfCountryRegionCode = dataReader.GetOrdinal("CountryRegionCode");
						    int indexOfIsOnlyStateProvinceFlag = dataReader.GetOrdinal("IsOnlyStateProvinceFlag");
						    int indexOfName = dataReader.GetOrdinal("Name");
						    int indexOfTerritoryID = dataReader.GetOrdinal("TerritoryID");
						    int indexOfrowguid = dataReader.GetOrdinal("rowguid");
						    int indexOfModifiedDate = dataReader.GetOrdinal("ModifiedDate");
						    
							int rowCount = 0;


							while (dataReader.Read() && rowCount < 1000 /*easy replace*/)
							{
								if(rowCount > 0 && (rowCount % 100) == 0)
								{
									Console.WriteLine("AdventureWorks2008R2:Person:StateProvince: {0}", rowCount);
								}

								if(rowCount > 0 && (rowCount % 1000) == 0)
								{
									Console.WriteLine("Comitting...");
									client.Transaction.Commit();
									client.Transaction.Enlist();
								}

								try
								{
									client.Document.Create("AdventureWorks2008R2:Person:StateProvince", new Models.Person_StateProvince
									{
											StateProvinceID= dataReader.GetInt32(indexOfStateProvinceID),
											StateProvinceCode= dataReader.GetString(indexOfStateProvinceCode),
											CountryRegionCode= dataReader.GetString(indexOfCountryRegionCode),
											IsOnlyStateProvinceFlag= dataReader.GetBoolean(indexOfIsOnlyStateProvinceFlag),
											Name= dataReader.GetString(indexOfName),
											TerritoryID= dataReader.GetInt32(indexOfTerritoryID),
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

