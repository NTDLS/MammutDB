using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Mammut.Client;
using Mammut.Common.Payload.Model;

namespace Mammut.TestHarness.Repository
{
	public partial class dbo_ErrorLogRepository
	{        
		public void Export_dbo_ErrorLog()
		{
            using (var client = new MammutClient("https://localhost:5001", "root", "p@ssWord!"))
			{

            //if(client.Schema.Exists("AdventureWorks2008R2:dbo:ErrorLog"))
			//{
			//	return;
			//}

            client.Transaction.Enlist();

            client.Schema.CreateAll("AdventureWorks2008R2:dbo:ErrorLog");

			using (SqlConnection connection = new SqlConnection("Server=.;Database=AdventureWorks2008R2;Trusted_Connection=True;"))
			{
				connection.Open();

				try
				{
					using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.ErrorLog", connection))
					{
						command.CommandTimeout = 10000;
						command.CommandType = System.Data.CommandType.Text;

						using (SqlDataReader dataReader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
						{
                            int indexOfErrorLogID = dataReader.GetOrdinal("ErrorLogID");
						    int indexOfErrorTime = dataReader.GetOrdinal("ErrorTime");
						    int indexOfUserName = dataReader.GetOrdinal("UserName");
						    int indexOfErrorNumber = dataReader.GetOrdinal("ErrorNumber");
						    int indexOfErrorSeverity = dataReader.GetOrdinal("ErrorSeverity");
						    int indexOfErrorState = dataReader.GetOrdinal("ErrorState");
						    int indexOfErrorProcedure = dataReader.GetOrdinal("ErrorProcedure");
						    int indexOfErrorLine = dataReader.GetOrdinal("ErrorLine");
						    int indexOfErrorMessage = dataReader.GetOrdinal("ErrorMessage");
						    
							int rowCount = 0;


							while (dataReader.Read() && rowCount < 1000 /*easy replace*/)
							{
								if(rowCount > 0 && (rowCount % 100) == 0)
								{
									Console.WriteLine("AdventureWorks2008R2:dbo:ErrorLog: {0}", rowCount);
								}

								if(rowCount > 0 && (rowCount % 1000) == 0)
								{
									Console.WriteLine("Comitting...");
									client.Transaction.Commit();
									client.Transaction.Enlist();
								}

								try
								{
									client.Document.Create("AdventureWorks2008R2:dbo:ErrorLog", new Models.dbo_ErrorLog
									{
											ErrorLogID= dataReader.GetInt32(indexOfErrorLogID),
											ErrorTime= dataReader.GetDateTime(indexOfErrorTime),
											UserName= dataReader.GetString(indexOfUserName),
											ErrorNumber= dataReader.GetInt32(indexOfErrorNumber),
											ErrorSeverity= dataReader.GetNullableInt32(indexOfErrorSeverity),
											ErrorState= dataReader.GetNullableInt32(indexOfErrorState),
											ErrorProcedure= dataReader.GetNullableString(indexOfErrorProcedure),
											ErrorLine= dataReader.GetNullableInt32(indexOfErrorLine),
											ErrorMessage= dataReader.GetString(indexOfErrorMessage),
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

