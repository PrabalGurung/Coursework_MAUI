
using System.Data.SQLite;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Database_Filter
{
	private SQLiteConnection _connection;
	string connectionString = "Data Source=mydatabase.db;Version=3;";

	public Database_Filter()
	{
		_connection = new SQLiteConnection(connectionString);
		_connection.Open();
	}
	public void Close()
	{
		_connection.Close();
	}

	public void FilterType(int userId, string type)
	{
		string query = @"
			SELECT date, amount, source, NULL, type, NULL, NULL
			FROM inflows 
			WHERE userId = @userId AND type = @type UNION
			SELECT date, amount, source, NULL, NULL, type, NULL
			FROM outflows 
			WHERE userId = @userId AND type = @type UNION
			SELECT date, amount, source, outstanding_amount, NULL, NULL, type
			FROM debts
			WHERE userId = @userId AND type = @type
			ORDER BY date;
		";

		using (var cmd = new SQLiteCommand(query, _connection))
		{
			cmd.Parameters.AddWithValue("@userId", userId);
			cmd.Parameters.AddWithValue("@type", type);

			using (var reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					Current_Account._currentAccount._transaction.Add(new Transaction
					{
						_balance = reader.GetInt32(1),
						_source = reader.GetString(2),
						_outflowType = (option.Cash_Outflow)Enum.Parse(typeof(option.Cash_Outflow), reader.GetString(4)),
						_inflowType = (option.Cash_Inflow)Enum.Parse(typeof(option.Cash_Inflow), reader.GetString(5)),
						_debt = reader.GetString(6),
						_used = reader.GetInt32(3),
						_date = DateOnly.Parse(reader.GetString(0))
					});
				}
			}
		}
	}

	public void FilterTags(int userId, string tags)
	{
		
	}

    public void FilterSearch(int userId, DateOnly firstDate, DateOnly lastDate, string incomeType, string outcomeType, string debtType, string tags, string order, string title)
    {
        string query = @"
		SELECT date, amount, source, 0, type, 'choose', 'choose', IFNULL(t.name, '')
		FROM inflows i JOIN tags t ON i.tagId = t.id
		WHERE i.userId = 1 
			AND i.date BETWEEN @firstDate AND @lastDate
			AND i.type LIKE CONCAT('%', @incomeType, '%')
			AND t.name LIKE CONCAT('%', @TagName, '%')
			AND i.source LIKE CONCAT('%', @source, '%')
		UNION ALL
		SELECT date, amount, source, 0, 'choose', type, 'choose', IFNULL(t.name, '')
		FROM outflows o JOIN tags t ON o.tagId = t.id
		WHERE o.userId = 1 
			AND o.date BETWEEN @firstDate AND @lastDate
			AND o.type LIKE CONCAT('%', @outflowType, '%')
			AND t.name LIKE CONCAT('%', @TagName, '%')
			AND o.source LIKE CONCAT('%', @source, '%')
		UNION ALL
		SELECT date, amount, source, outstanding_amount, 'choose', 'choose', type, IFNULL(t.name, '')
		FROM debts d JOIN tags t ON d.tagId = t.id
		WHERE d.userId = 1 
			AND d.date BETWEEN @firstDate AND @lastDate
			AND d.type LIKE CONCAT('%', @debtType, '%')
			AND t.name LIKE CONCAT('%', @TagName, '%')
			AND d.source LIKE CONCAT('%', @source, '%')
		" + order;
		Console.WriteLine(query);

        using (var cmd = new SQLiteCommand(query, _connection))
        {
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@firstDate", firstDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@lastDate", lastDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@incomeType", incomeType);
			cmd.Parameters.AddWithValue("@outflowType", outcomeType);
			cmd.Parameters.AddWithValue("@debtType", debtType);
			cmd.Parameters.AddWithValue("@TagName", tags);
			cmd.Parameters.AddWithValue("@source", title);

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
                        _debt = reader.GetString(6),
						_tags = reader.GetString(7)
                    });
                }
            }
        }
    }
}
