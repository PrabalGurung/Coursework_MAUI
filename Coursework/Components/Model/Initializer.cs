using System;
using System.Data.Common;
using System.Data.SQLite;
using System.Xml.Linq;

public class Initializer
{
	string connectionString = "Data Source=mydatabase.db;Version=3;";
	public void CheckDatabase()
	{
		if (File.Exists("mydatabase.db"))
		{
			Console.WriteLine("Database file exists.");
		}
		else
		{
			Console.WriteLine("Database file does not exist.");
			CreateTable();
			Database_Manager dm = new Database_Manager();
			dm.InsertUser("Skybird", "123", 0);
			dm.Close();
		}
	}

	public void CreateTable()
	{
		string createUserQuery = @"
            CREATE TABLE users (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT NOT NULL,
                password TEXT NOT NULL,
				balance INTEGER
            );"; 

		string createInflowQuery = @"
            CREATE TABLE inflows (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
				userId INTEGER,
				amount INTEGER NOT NULL,
				source TEXT NOT NULL,
				date DATE NOT NULL,
				type TEXT NOT NULL,
				tagId INTEGER,
				description TEXT,
				FOREIGN KEY (userId) REFERENCES users(id)
				FOREIGN KEY (tagId) REFERENCES tags(id)
            );";

		string createOutflowQuery = @"
            CREATE TABLE outflows (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
				userId INTEGER,
				amount INTEGER NOT NULL,
				source TEXT NOT NULL,
				date DATE NOT NULL,
				type TEXT NOT NULL,				
				tagId INTEGER,
				description TEXT,
				FOREIGN KEY (userId) REFERENCES user(id)
				FOREIGN KEY (tagId) REFERENCES tags(id)
            );";

		string createDebtQuery = @"
            CREATE TABLE debts (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
				userId INTEGER,
				amount INTEGER NOT NULL,
				outstanding_amount INTEGER NOT NULL,
				source TEXT NOT NULL,
				date DATE NOT NULL,
				type TEXT NOT NULL,
				tagId INTEGER,
				description TEXT,
				FOREIGN KEY (userId) REFERENCES user(id)
				FOREIGN KEY (tagId) REFERENCES tags(id)
            );";

		string createDUQuery = @"
            CREATE TABLE DUs (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
				userId INTEGER,
				inflowId INTEGER,
				debtId INTEGER,
				usedAmount INTEGER,
				FOREIGN KEY (userId) REFERENCES user(id)
				FOREIGN KEY (debtId) REFERENCES debt(id)
				FOREIGN KEY (inflowId) REFERENCES inflow(id)
		);";

		string createTags = @"
		CREATE TABLE tags (
			id INTEGER PRIMARY KEY AUTOINCREMENT,
			name TEXT);
		";

		using (SQLiteConnection connection = new SQLiteConnection(connectionString))
		{
			connection.Open();

			using (SQLiteCommand command = new SQLiteCommand(createUserQuery, connection))
			{
				command.ExecuteNonQuery();
			}

			using (SQLiteCommand command = new SQLiteCommand(createInflowQuery, connection))
			{
				command.ExecuteNonQuery();
			}

			using (SQLiteCommand command = new SQLiteCommand(createOutflowQuery, connection))
			{
				command.ExecuteNonQuery();
			}

			using (SQLiteCommand command = new SQLiteCommand(createDebtQuery, connection))
			{
				command.ExecuteNonQuery();
			}
			using (SQLiteCommand command = new SQLiteCommand(createDUQuery, connection))
			{
				command.ExecuteNonQuery();
			}
            using (SQLiteCommand command = new SQLiteCommand(createTags, connection))
            {
                command.ExecuteNonQuery();
            }

            connection.Close();
		}
	}

	public void DeleteTable()
	{
		string dropUserQuery = "DROP TABLE user;";
		string dropInflowQuery = "DROP TABLE inflow;";
		string dropOutflowQuery = "DROP TABLE outflow;";
		string dropDebtQuery = "DROP TABLE debt;";

		using (SQLiteConnection connection = new SQLiteConnection(connectionString))
		{
			connection.Open();

			using (SQLiteCommand command = new SQLiteCommand(dropUserQuery, connection))
			{
				command.ExecuteNonQuery();
			}

			using (SQLiteCommand command = new SQLiteCommand(dropInflowQuery, connection))
			{
				command.ExecuteNonQuery();
			}

			using (SQLiteCommand command = new SQLiteCommand(dropOutflowQuery, connection))
			{
				command.ExecuteNonQuery();
			}

			using (SQLiteCommand command = new SQLiteCommand(dropDebtQuery, connection))
			{
				command.ExecuteNonQuery();
			}

			connection.Close();
		}
	}
}
