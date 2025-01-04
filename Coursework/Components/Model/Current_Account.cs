using System.Net.NetworkInformation;

public class Current_Account
{
	public static Finance_Handler _currentAccount { get; set; }
    public static List<Transaction> _tags { get; set; } = new List<Transaction>();

    public static void DatabaseExtraction()
    {
        DropTable();
        Database_Selection databaseSelection = new Database_Selection();
        databaseSelection.GetInflows(1);
        databaseSelection.GetOutflows(1);
        databaseSelection.GetDebts(1);
        databaseSelection.Close();
    }

    public static void DropTable()
    {
        _currentAccount._transaction.Clear();
    }

    public static void SortDate(int date)
    {
        Database_Selection database_Selection = new Database_Selection();
        database_Selection.GetDate(1);
    }

    public static void Filter(int userId, DateOnly firstDate, DateOnly lastDate, string incomeType, string outcomeType, string debtType, string tags, string order)
    {
        Database_Filter database_filter = new Database_Filter();
        database_filter.FilterSearch(userId, firstDate, lastDate, incomeType, outcomeType, debtType, tags, order);
    }

    public static void Tags()
    {
        Database_Selection ds = new Database_Selection();
        ds.GetTags();
    }
}