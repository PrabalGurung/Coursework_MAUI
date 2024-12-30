﻿using System.Data.SQLite;

public class Database_Manager
{
	private SQLiteConnection _connection;
	string connectionString = "Data Source=mydatabase.db;Version=3;";

	public Database_Manager()
	{
		_connection = new SQLiteConnection(connectionString);
		_connection.Open();
	}

	public void InsertUser(string name, string password, int balance)
	{
		string query = "INSERT INTO users (name, password, balance) VALUES (@name, @password, @balance)";
		using (var cmd = new SQLiteCommand(query, _connection))
		{
			cmd.Parameters.AddWithValue("@name", name);
			cmd.Parameters.AddWithValue("@password", password);
			cmd.Parameters.AddWithValue("@balance", balance);
			cmd.ExecuteNonQuery();
		}
	}

	public void UpdateUserBalance(int userId, int newBalance)
	{
		string query = "UPDATE users SET balance = @balance WHERE id = @userId";
		using (var cmd = new SQLiteCommand(query, _connection))
		{
			cmd.Parameters.AddWithValue("@balance", newBalance);
			cmd.Parameters.AddWithValue("@userId", userId);
			cmd.ExecuteNonQuery();
		}
	}

	public void IncreaseBalance(int userId, int amount)
	{
		string query = "UPDATE users SET balance = balance + @amount WHERE id = @userId";

		using (var cmd = new SQLiteCommand(query, _connection))
		{
			cmd.Parameters.AddWithValue("@amount", amount);
			cmd.Parameters.AddWithValue("@userId", userId);

			cmd.ExecuteNonQuery();
		}
	}

	public void DecreaseBalance(int userId, int amount)
	{
		string query = "UPDATE users SET balance = balance - @amount WHERE id = @userId";
		using (var cmd = new SQLiteCommand(query, _connection))
		{
			cmd.Parameters.AddWithValue("@amount", amount);
			cmd.Parameters.AddWithValue("@userId", userId);
			cmd.ExecuteNonQuery();
		}
	}

	public int InsertInflow(int userId, int amount, string source, string date, string type)
	{
		string query = "INSERT INTO inflows (userId, amount, source, date, type) VALUES (@userId, @amount, @source, @date, @type)";
		using (var cmd = new SQLiteCommand(query, _connection))
		{
			cmd.Parameters.AddWithValue("@userId", userId);
			cmd.Parameters.AddWithValue("@amount", amount);
			cmd.Parameters.AddWithValue("@source", source);
			cmd.Parameters.AddWithValue("@date", date);
			cmd.Parameters.AddWithValue("@type", type);
			cmd.ExecuteNonQuery();
		}
		long lastInsertId = _connection.LastInsertRowId;

		return (int)lastInsertId;
	}

	public void InsertOutflow(int userId, int amount, string source, string date, string type)
	{
		string query = "INSERT INTO outflows (userId, amount, source, date, type) VALUES (@userId, @amount, @source, @date, @type)";
		using (var cmd = new SQLiteCommand(query, _connection))
		{
			cmd.Parameters.AddWithValue("@userId", userId);
			cmd.Parameters.AddWithValue("@amount", amount);
			cmd.Parameters.AddWithValue("@source", source);
			cmd.Parameters.AddWithValue("@date", date);
			cmd.Parameters.AddWithValue("@type", type);
			cmd.ExecuteNonQuery();
		}
	}

	public void InsertDebt(int userId, int amount, string source, string date, string type)
	{
		string query = "INSERT INTO debts (userId, outstanding_amount, amount, source, date, type) VALUES (@userId, @outstanding_amount, @amount, @source, @date, @type)";
		using (var cmd = new SQLiteCommand(query, _connection))
		{
			cmd.Parameters.AddWithValue("@userId", userId);
			cmd.Parameters.AddWithValue("@outstanding_amount", amount);
			cmd.Parameters.AddWithValue("@amount", amount);
			cmd.Parameters.AddWithValue("@source", source);
			cmd.Parameters.AddWithValue("@date", date);
			cmd.Parameters.AddWithValue("@type", type);
			cmd.ExecuteNonQuery();
		}
	}

	public void UpdateDebt(int debtId, int inFlowId, int inflowAmount, string newType)
	{
		string query = "UPDATE debts SET inflowId = @inFlowId, type = @type, clearedAmount = @clearedAmount WHERE id = @debtId";
		using (var cmd = new SQLiteCommand(query, _connection))
		{
			cmd.Parameters.AddWithValue("@inFlowId", inFlowId);
			cmd.Parameters.AddWithValue("@clearedAmount", inflowAmount);
			cmd.Parameters.AddWithValue("@type", newType);
			cmd.Parameters.AddWithValue("@debtId", debtId);
			cmd.ExecuteNonQuery();
		}
	}

	public void Close()
	{
		_connection.Close();
	}
}

