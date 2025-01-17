
using System.Data.SQLite;
using static System.Runtime.InteropServices.JavaScript.JSType;

/* --
 * Handles selection from database
 * Passes parameter to static list field present in Current_account.cs
 --*/
public class Database_Selection
{
	// declaration
	private SQLiteConnection _connection;
	string connectionString = "Data Source=mydatabase.db;Version=3;";

	// no parameterized connection
	public Database_Selection()
	{
		_connection = new SQLiteConnection(connectionString);
		_connection.Open();
	}

	// Extracts users from users table with the given id
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

	// Extracts inflows from inflows table with the given id
	public void GetInflows(int userId)
	{
		string query = "SELECT i.*, t.name FROM inflows i JOIN tags t ON i.tagId = t.id WHERE userId = @userId";

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
						_date = DateOnly.Parse(reader.GetString(4)),
						_description = reader.GetString(7),
						_tags = reader.GetString(8)
					});
				}
			}
		}
	}

	// Extracts outflow from outflow table with given id
	public void GetOutflows(int userId)
	{
		string query = "SELECT o.*, t.name FROM outflows o JOIN tags t ON o.tagId = t.id WHERE userId = @userId";

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
						_date = DateOnly.Parse(reader.GetString(4)),
						_description = reader.GetString(7),
						_tags = reader.GetString(8)
					});
				}
			}
		}
	}

	// Extracts debt from debts table with given id
	public void GetDebts(int userId)
	{
		string query = "SELECT d.amount, d.type, d.source, d.date, t.name, d.description FROM debts d JOIN tags t ON d.tagId = t.id WHERE userId = @userId";

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
						_date = DateOnly.Parse(reader.GetString(3)),
						_tags = reader.GetString(4),
						_description = reader.GetString(5)
					});
				}
			}
		}
	}

	// Orders by date of transaction table
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

	// Extracts tags from tag table
	public void GetTags()
	{
		string query = "SELECT name FROM tags";

		using (var cmd = new SQLiteCommand(query, _connection))
		{
			using (var reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					Current_Account._tags.Add(new Transaction
					{
						_tags = reader.GetString(0)
					});
				}
			}
		}
	}

	//  Extracts user, inflow and debt relation from dus table
	public void GetDebtHistory()
	{
		string query = @"
		SELECT u.name, IFNULL(dus.debtId, 0), IFNULL(dus.inflowId, 0), i.source, IFNULL(d.outstanding_amount, 0) ,dus.usedAmount FROM DUS 
		JOIN debts d 
			ON DUS.debtId = d.id 
		JOIN users u
			ON DUS.userId = u.id
		JOIN inflows i
			ON DUS.inflowId = i.id
		";

		using (var cmd =  new SQLiteCommand(query, _connection))
		{
			using(var reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					Current_Account.dUs.Add(new DUS
					{
						user = reader.GetString(0),
						debt = reader.GetInt32(1),
						inflow = reader.GetInt32(2),
						source = reader.GetString(3),
						outstandingAmount = reader.GetInt32(4),
						userAmount = reader.GetInt32(5)
					});
				}
			}
		}

	}

	// Closes connection
	public void Close()
	{
		_connection.Close();
	}
}
