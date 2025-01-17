using System.Net.NetworkInformation;

// Model for account
public class Current_Account
{
    // declaration
	public static Finance_Handler _currentAccount { get; set; }
    public static List<Transaction> _tags { get; set; } = new List<Transaction>();
    public static List<DUS> dUs { get; set; } = new List<DUS>();
    
    // Debt history extraction 
    public static void DebtHistoryExtraction()
    {
        DropHistory();
        Database_Selection databaseSelection = new Database_Selection();
        databaseSelection.GetDebtHistory();
        databaseSelection.Close();
    }

    // Database extraction
    public static void DatabaseExtraction()
    {
        DropTable();
        Database_Selection databaseSelection = new Database_Selection();
        databaseSelection.GetInflows(1);
        databaseSelection.GetOutflows(1);
        databaseSelection.GetDebts(1);
        databaseSelection.Close();
    }

    // Clear list current account
    public static void DropTable()
    {
        _currentAccount._transaction.Clear();
    }

    // Sort date 
    public static void SortDate(int date)
    {
        Database_Selection database_Selection = new Database_Selection();
        database_Selection.GetDate(1);
    }

    // Sorting filtering and searching logic handler before database
    public static void Filter(int userId, DateOnly firstDate, DateOnly lastDate, string incomeType, string outcomeType, string debtType, string tags, string order, string _title)
    {
		if (string.Equals(order, "date"))
        {
            order = " Order By date";
        } else if (string.Equals(order, "source"))
        {
            order = " Order By source";
        } else if (string.Equals(order, "tags"))
        {
            order = " Order By tags";
        } else
        {
            order = "";
        }

        if (string.Equals(incomeType, "choose"))
        {
            incomeType = "";
        }   
        
        if (string.Equals(outcomeType, "choose"))
        {
            outcomeType = "";
        }

        Console.WriteLine(order);
        Database_Filter database_filter = new Database_Filter();
        database_filter.FilterSearch(userId, firstDate, lastDate, incomeType, outcomeType, debtType, tags, order, _title);
    }

    // Clears history
    public static void DropHistory()
    {
        dUs.Clear();
    }

    // Tags extraction logic before database logic handle
    public static void Tags()
    {
        Database_Selection ds = new Database_Selection();
        ds.GetTags();
        ds.Close();
    }       
}