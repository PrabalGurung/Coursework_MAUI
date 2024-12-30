
using System.Data.SQLite;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Database_Selection
{
	private SQLiteConnection _connection;
	string connectionString = "Data Source=mydatabase.db;Version=3;";

	public Database_Selection()
	{
		_connection = new SQLiteConnection(connectionString);
		_connection.Open();
	}

	public int GetBalance(int userId)
	{
		string query = "SELECT * FROM users WHERE id = @userId;";

		using (var cmd = new SQLiteCommand(query, _connection))
		{
			cmd.Parameters.AddWithValue("@userId", userId);

			using (var reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					return reader.GetInt32(3);
				}
				return 0;
			}
		}
	}

	public void GetInflows(int userId)
	{
		string query = "SELECT * FROM inflows WHERE userId = @userId";

		using (var cmd = new SQLiteCommand(query, _connection))
		{
			cmd.Parameters.AddWithValue("@userId", userId);

			using (var reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					Current_Account._currentAccount._transaction.Add(new Transaction
					{
						_balance = reader.GetInt32(2),
						_inflowType = (option.Cash_Inflow)Enum.Parse(typeof(option.Cash_Inflow), reader.GetString(5)),
						_source = reader.GetString(3),
						_date = DateOnly.Parse(reader.GetString(4))
					});
				}
			}
		}
	}

	public void GetOutflows(int userId)
	{
		string query = "SELECT * FROM outflows WHERE userId = @userId";

		using (var cmd = new SQLiteCommand(query, _connection))
		{
			cmd.Parameters.AddWithValue("@userId", userId);

			using (var reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					Current_Account._currentAccount._transaction.Add(new Transaction
					{
						_balance = reader.GetInt32(2),
						_outflowType = (option.Cash_Outflow)Enum.Parse(typeof(option.Cash_Outflow), reader.GetString(5)),
						_source = reader.GetString(3),
						_date = DateOnly.Parse(reader.GetString(4))
					});
				}
			}
		}
	}

	public void GetDebts(int userId)
	{
		string query = "SELECT amount, type, source, date FROM debts WHERE userId = @userId";

		using (var cmd = new SQLiteCommand(query, _connection))
		{
			cmd.Parameters.AddWithValue("@userId", userId);

			using (var reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					Current_Account._currentAccount._transaction.Add(new Transaction
					{
						_balance = reader.GetInt32(0),
						_debt = reader.GetString(1),
						_source = reader.GetString(2),
						_date = DateOnly.Parse(reader.GetString(3))
					});
				}
			}
		}
	}

	public void GetDate(int userId)
	{
		string query = @"
			SELECT date, amount, source, 0, type, 'choose', 'choose'
			FROM inflows 
			WHERE userId = @userId UNION ALL
			SELECT date, amount, source, 0, 'choose', type, 'choose'
			FROM outflows 
			WHERE userId = @userId UNION ALL
			SELECT date, amount, source, outstanding_amount, 'choose', 'choose', type
			FROM debts
			WHERE userId = @userId
			ORDER BY date;
		";

		using (var cmd = new SQLiteCommand(query, _connection))
		{
			cmd.Parameters.AddWithValue("@userId", userId);

			using (var reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
                    Current_Account._currentAccount._transaction.Add(new Transaction
                    {
                        _date = DateOnly.Parse(reader.GetString(0)),
                        _balance = reader.GetInt32(1),
                        _source = reader.GetString(2),
                        _used = reader.GetInt32(3),
                        _inflowType = (option.Cash_Inflow)Enum.Parse(typeof(option.Cash_Inflow), reader.GetString(4)),
                        _outflowType = (option.Cash_Outflow)Enum.Parse(typeof(option.Cash_Outflow), reader.GetString(5)),
                        _debt = reader.GetString(6)
                    });
                }
			}
		}
	}

	public void Close()
	{
		_connection.Close();
	}
}
