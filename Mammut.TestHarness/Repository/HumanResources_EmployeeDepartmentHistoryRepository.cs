using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Mammut.Client;
using Mammut.Common.Payload.Model;

namespace Mammut.TestHarness.Repository
{
	public partial class HumanResources_EmployeeDepartmentHistoryRepository
	{        
		public void Export_HumanResources_EmployeeDepartmentHistory()
		{
            using (var client = new MammutClient("https://localhost:5001", "root", "p@ssWord!"))
			{

            //if(client.Schema.Exists("AdventureWorks2008R2:HumanResources:EmployeeDepartmentHistory"))
			//{
			//	return;
			//}

            client.Transaction.Enlist();

            client.Schema.CreateAll("AdventureWorks2008R2:HumanResources:EmployeeDepartmentHistory");

			using (SqlConnection connection = new SqlConnection("Server=.;Database=AdventureWorks2008R2;Trusted_Connection=True;"))
			{
				connection.Open();

				try
				{
					using (SqlCommand command = new SqlCommand("SELECT * FROM HumanResources.EmployeeDepartmentHistory", connection))
					{
						command.CommandTimeout = 10000;
						command.CommandType = System.Data.CommandType.Text;

						using (SqlDataReader dataReader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
						{
                            int indexOfBusinessEntityID = dataReader.GetOrdinal("BusinessEntityID");
						    int indexOfDepartmentID = dataReader.GetOrdinal("DepartmentID");
						    int indexOfShiftID = dataReader.GetOrdinal("ShiftID");
						    int indexOfStartDate = dataReader.GetOrdinal("StartDate");
						    int indexOfEndDate = dataReader.GetOrdinal("EndDate");
						    int indexOfModifiedDate = dataReader.GetOrdinal("ModifiedDate");
						    
							int rowCount = 0;


							while (dataReader.Read() && rowCount < 1000 /*easy replace*/)
							{
								if(rowCount > 0 && (rowCount % 100) == 0)
								{
									Console.WriteLine("AdventureWorks2008R2:HumanResources:EmployeeDepartmentHistory: {0}", rowCount);
								}

								if(rowCount > 0 && (rowCount % 1000) == 0)
								{
									Console.WriteLine("Comitting...");
									client.Transaction.Commit();
									client.Transaction.Enlist();
								}

								try
								{
									client.Document.Create("AdventureWorks2008R2:HumanResources:EmployeeDepartmentHistory", new Models.HumanResources_EmployeeDepartmentHistory
									{
											BusinessEntityID= dataReader.GetInt32(indexOfBusinessEntityID),
											DepartmentID= dataReader.GetInt16(indexOfDepartmentID),
											ShiftID= dataReader.GetByte(indexOfShiftID),
											StartDate= dataReader.GetDateTime(indexOfStartDate),
											EndDate= dataReader.GetNullableDateTime(indexOfEndDate),
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

