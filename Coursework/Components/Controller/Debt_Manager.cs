
using System.Data.SqlClient;
using System.Data.SQLite;

public class Debt_Manager
{
	private SQLiteConnection _connection;
	string connectionString = "Data Source=mydatabase.db;Version=3;";

	public Debt_Manager()
	{
		_connection = new SQLiteConnection(connectionString);
		_connection.Open();
	}

	public void Debt_Collector(int amount, int userId, int inflowId)
	{
		string updateQuery = "UPDATE debts SET outstanding_amount = outstanding_amount - @amount WHERE id = @id;";
		string updateQueryZero = "UPDATE debts SET outstanding_amount = 0 WHERE id = @id;";
		string updateQueryType = "UPDATE debts SET type = 'clear' WHERE id = @id;";
		string selectQuery = "SELECT id, outstanding_amount FROM debts WHERE userId = @userId;";
		string insertQuery = "INSERT INTO DUs (userId, debtId, usedAmount, inflowId) VALUES (@userId, @debtId, @usedAmount, @inflowId)";

		using (var cmd = new SQLiteCommand(selectQuery, _connection))
		{
			cmd.Parameters.AddWithValue("@userId", userId);

			using (var reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					int debtId = reader.GetInt32(reader.GetOrdinal("id"));
					int currentDebt = reader.GetInt32(reader.GetOrdinal("outstanding_amount"));

					if (currentDebt > 0)
					{
						if (currentDebt <= amount)
						{
							// fully paid
							using (var updateCmd = new SQLiteCommand(updateQueryZero, _connection))
							{
								updateCmd.Parameters.AddWithValue("@id", debtId);

								int rowsAffected = updateCmd.ExecuteNonQuery();

								using (var typeUpdateCmd = new SQLiteCommand(updateQueryType, _connection))
								{
									typeUpdateCmd.Parameters.AddWithValue("@id", debtId);
									typeUpdateCmd.ExecuteNonQuery();
								}
							}
							using (var insertCmd = new SQLiteCommand(insertQuery, _connection))
							{
								insertCmd.Parameters.AddWithValue("@userId", userId);
								insertCmd.Parameters.AddWithValue("@debtId", debtId);
								insertCmd.Parameters.AddWithValue("@usedAmount", currentDebt);
								insertCmd.Parameters.AddWithValue("@inflowId", inflowId);
								insertCmd.ExecuteNonQuery();
							}
							amount -= currentDebt;
						}
						else
						{
							//partial reduction
							using (var updateCmd = new SQLiteCommand(updateQuery, _connection))
							{
								updateCmd.Parameters.AddWithValue("@amount", amount);
								updateCmd.Parameters.AddWithValue("@userId", userId);
								updateCmd.Parameters.AddWithValue("@id", debtId);

								int rowsAffected = updateCmd.ExecuteNonQuery();
							}
							using (var insertCmd = new SQLiteCommand(insertQuery, _connection))
							{
								insertCmd.Parameters.AddWithValue("@userId", userId);
								insertCmd.Parameters.AddWithValue("@debtId", debtId);
								insertCmd.Parameters.AddWithValue("@inflowId", inflowId);
								insertCmd.Parameters.AddWithValue("@usedAmount", currentDebt);
								insertCmd.ExecuteNonQuery();
							}
							break;
						}
						if (amount <= 0)
						{
							break;
						}
					}
				}
			}
			Console.WriteLine("Axa");
			Database_Manager database_Manager = new Database_Manager();
			database_Manager.IncreaseBalance(userId, amount);
		}
	}

}


