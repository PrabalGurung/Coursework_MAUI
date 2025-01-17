using System.Data.SQLite;
using System.Xml.Linq;

/* --
 * Handles all insertion to database
 * Handles updates in database
 --*/
public class Database_Manager
{
	// declaration
	private SQLiteConnection _connection;
	string connectionString = "Data Source=mydatabase.db;Version=3;";

	// no parameterized constructor
	public Database_Manager()
	{
		_connection = new SQLiteConnection(connectionString);
		_connection.Open();
	}

	// Insert data in user table
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

	// Insert data in tag table
	public void InsertDefaultTags()
	{
		string query = "INSERT INTO Tags(name) VALUES ('No Tags'), ('Yearly'), ('Monthly'), ('Food'), ('Drinks'), ('Clothes'), ('Gadgets'), ('Miscellaneous'), ('Fuel'), ('Rent'), ('EMI'), ('Party')";
		using (var cmd = new SQLiteCommand(query, _connection))
		{
			cmd.ExecuteNonQuery();
		}
	}

	// Update balance in update user balance
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

	// Increases balance in given user 
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

	// Decrease balance in given user
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

	// Insert value inflows table
	public int InsertInflow(int userId, int amount, string source, string date, string type, int index, string description)
	{
		string query = "INSERT INTO inflows (userId, amount, source, date, type, tagId, description) VALUES (@userId, @amount, @source, @date, @type, @tagId, @description)";
		using (var cmd = new SQLiteCommand(query, _connection))
		{
			cmd.Parameters.AddWithValue("@userId", userId);
			cmd.Parameters.AddWithValue("@amount", amount);
			cmd.Parameters.AddWithValue("@source", source);
			cmd.Parameters.AddWithValue("@date", date);
			cmd.Parameters.AddWithValue("@type", type);
			cmd.Parameters.AddWithValue("@tagId", index);
			cmd.Parameters.AddWithValue("@description", description);
			cmd.ExecuteNonQuery();
		}
		long lastInsertId = _connection.LastInsertRowId;

		return (int)lastInsertId;
	}

	// Insert value in outflows table
	public void InsertOutflow(int userId, int amount, string source, string date, string type, int index, string description)
	{
		string query = "INSERT INTO outflows (userId, amount, source, date, type, tagId, description) VALUES (@userId, @amount, @source, @date, @type, @tagId, @description)";
		using (var cmd = new SQLiteCommand(query, _connection))
		{
			cmd.Parameters.AddWithValue("@userId", userId);
			cmd.Parameters.AddWithValue("@amount", amount);
			cmd.Parameters.AddWithValue("@source", source);
			cmd.Parameters.AddWithValue("@date", date);
			cmd.Parameters.AddWithValue("@type", type);
			cmd.Parameters.AddWithValue("@tagId", index);
			cmd.Parameters.AddWithValue("@description", description);
			cmd.ExecuteNonQuery();
		}
	}

	// Insert value in debt table
	public void InsertDebt(int userId, int amount, string source, string date, string type, int index, string description)
	{
		Console.WriteLine("Tag:" + index);
		string query = "INSERT INTO debts (userId, outstanding_amount, amount, source, date, type, tagId, description) VALUES (@userId, @outstanding_amount, @amount, @source, @date, @type, @index, @description)";
		using (var cmd = new SQLiteCommand(query, _connection))
		{
			cmd.Parameters.AddWithValue("@userId", userId);
			cmd.Parameters.AddWithValue("@outstanding_amount", amount);
			cmd.Parameters.AddWithValue("@amount", amount);
			cmd.Parameters.AddWithValue("@source", source);
			cmd.Parameters.AddWithValue("@date", date);
			cmd.Parameters.AddWithValue("@type", type);
			cmd.Parameters.AddWithValue("@index", index);
			cmd.Parameters.AddWithValue("@description", description);
			cmd.ExecuteNonQuery();
		}
	}

	// Insert value in tags table
	public void AddTags(string name)
	{
		string query = "INSERT INTO tags (name) VALUES (@name)";
		using (var cmd = new SQLiteCommand(query, _connection))
		{
			cmd.Parameters.AddWithValue("@name", name);
			cmd.ExecuteNonQuery();
		}
	}

	// Disconnect from database 
	public void Close()
	{
		_connection.Close();
	}
}

