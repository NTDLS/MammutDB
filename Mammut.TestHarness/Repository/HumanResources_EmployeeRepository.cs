using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Mammut.Client;
using Mammut.Common.Payload.Model;

namespace Mammut.TestHarness.Repository
{
	public partial class HumanResources_EmployeeRepository
	{        
		public void Export_HumanResources_Employee()
		{
            using (var client = new MammutClient("https://localhost:5001", "root", "p@ssWord!"))
			{

            //if(client.Schema.Exists("AdventureWorks2008R2:HumanResources:Employee"))
			//{
			//	return;
			//}

            client.Transaction.Enlist();

            client.Schema.CreateAll("AdventureWorks2008R2:HumanResources:Employee");

			using (SqlConnection connection = new SqlConnection("Server=.;Database=AdventureWorks2008R2;Trusted_Connection=True;"))
			{
				connection.Open();

				try
				{
					using (SqlCommand command = new SqlCommand("SELECT * FROM HumanResources.Employee", connection))
					{
						command.CommandTimeout = 10000;
						command.CommandType = System.Data.CommandType.Text;

						using (SqlDataReader dataReader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
						{
                            int indexOfBusinessEntityID = dataReader.GetOrdinal("BusinessEntityID");
						    int indexOfNationalIDNumber = dataReader.GetOrdinal("NationalIDNumber");
						    int indexOfLoginID = dataReader.GetOrdinal("LoginID");
						    int indexOfOrganizationNode = dataReader.GetOrdinal("OrganizationNode");
						    int indexOfOrganizationLevel = dataReader.GetOrdinal("OrganizationLevel");
						    int indexOfJobTitle = dataReader.GetOrdinal("JobTitle");
						    int indexOfBirthDate = dataReader.GetOrdinal("BirthDate");
						    int indexOfMaritalStatus = dataReader.GetOrdinal("MaritalStatus");
						    int indexOfGender = dataReader.GetOrdinal("Gender");
						    int indexOfHireDate = dataReader.GetOrdinal("HireDate");
						    int indexOfSalariedFlag = dataReader.GetOrdinal("SalariedFlag");
						    int indexOfVacationHours = dataReader.GetOrdinal("VacationHours");
						    int indexOfSickLeaveHours = dataReader.GetOrdinal("SickLeaveHours");
						    int indexOfCurrentFlag = dataReader.GetOrdinal("CurrentFlag");
						    int indexOfrowguid = dataReader.GetOrdinal("rowguid");
						    int indexOfModifiedDate = dataReader.GetOrdinal("ModifiedDate");
						    
							int rowCount = 0;


							while (dataReader.Read() && rowCount < 1000 /*easy replace*/)
							{
								if(rowCount > 0 && (rowCount % 100) == 0)
								{
									Console.WriteLine("AdventureWorks2008R2:HumanResources:Employee: {0}", rowCount);
								}

								if(rowCount > 0 && (rowCount % 1000) == 0)
								{
									Console.WriteLine("Comitting...");
									client.Transaction.Commit();
									client.Transaction.Enlist();
								}

								try
								{
									client.Document.Create("AdventureWorks2008R2:HumanResources:Employee", new Models.HumanResources_Employee
									{
											BusinessEntityID= dataReader.GetInt32(indexOfBusinessEntityID),
											NationalIDNumber= dataReader.GetString(indexOfNationalIDNumber),
											LoginID= dataReader.GetString(indexOfLoginID),
											OrganizationLevel= dataReader.GetNullableInt16(indexOfOrganizationLevel),
											JobTitle= dataReader.GetString(indexOfJobTitle),
											BirthDate= dataReader.GetDateTime(indexOfBirthDate),
											MaritalStatus= dataReader.GetString(indexOfMaritalStatus),
											Gender= dataReader.GetString(indexOfGender),
											HireDate= dataReader.GetDateTime(indexOfHireDate),
											SalariedFlag= dataReader.GetBoolean(indexOfSalariedFlag),
											VacationHours= dataReader.GetInt16(indexOfVacationHours),
											SickLeaveHours= dataReader.GetInt16(indexOfSickLeaveHours),
											CurrentFlag= dataReader.GetBoolean(indexOfCurrentFlag),
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

