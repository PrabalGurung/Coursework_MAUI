using System;
using System.Data.SQLite;
public class Inflow_Database
{
	string connectionString = "Data Source=mydatabase.db;Version=3;";
	public void OpenDatabaseConnection()
	{
		using (SQLiteConnection connection = new SQLiteConnection(connectionString))
		{
			connection.Open();
		}
	}

	public void CreateTable()
	{
		string createTableQuery = @"
            CREATE TABLE inflow (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT NOT NULL,
                email TEXT NOT NULL
            );";

		using (SQLiteConnection connection = new SQLiteConnection(connectionString))
		{
			connection.Open();

			using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
			{
				command.ExecuteNonQuery();
			}
		}
	}
}