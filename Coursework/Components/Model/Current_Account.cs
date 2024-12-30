public class Current_Account
{
	public static Finance_Handler _currentAccount { get; set; }

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
}