using System;
using System.Data;
using System.Data.SqlClient;


namespace CopyingTheDatabase
{
	class Program
	{
		static void Main(string[] args)
		{
			string sourceConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=EConsultingTest;Integrated Security=True";
			string copiedConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=CopiedDB1;Integrated Security=True;Pooling=False";
			SqlConnection sourceConnection = new SqlConnection(sourceConnectionString);
			SqlConnection copiedConnection = new SqlConnection(copiedConnectionString);
			try
			{
				sourceConnection.Open();
				copiedConnection.Open();
				DataTable t = sourceConnection.GetSchema("Tables");
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				sourceConnection.Close();
			}
		}
	}
}
