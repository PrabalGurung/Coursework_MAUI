using Coursework.Components.Pages;

public class Finance_Handler
{
	public string _name { get; set; }
	public int _balance { get; set; }
	public List<Transaction> _transaction { get; set; } = new List<Transaction>();

	Database_Manager dm;

	public Finance_Handler()
	{
	}

	public Finance_Handler(string name)
	{
		_name = name;
	}

	public bool History(Transaction t)
	{
		dm = new Database_Manager();
		option.Cash_Outflow _type = option.Cash_Outflow.choose;

		//debt
		if (t._debt != null )
		{
			Database_Manager database_Manager = new Database_Manager();
			database_Manager.IncreaseBalance(1, t._balance);
			Console.WriteLine("Tag: " + t._index);
			dm.InsertDebt(1, t._balance, t._source, t._date.ToString("yyyy-MM-dd"), t._debt.ToString(), t._index, t._description);
			return true;
		}
		else
		{
			//inflow
			if (t._outflowType == _type)
			{

				int iflowId = dm.InsertInflow(1, t._balance, t._source, t._date.ToString("yyyy-MM-dd"), t._inflowType.ToString(), t._index, t._description);
				DebtTrack(t._balance, iflowId);
				return true;
			}
			else
			{
				//outflow
				if (_balance > t._balance)
				{
					Database_Manager database_Manager = new Database_Manager();
					database_Manager.DecreaseBalance(1, t._balance);
					dm.InsertOutflow(1, t._balance, t._source, t._date.ToString("yyyy-MM-dd"), t._outflowType.ToString(), t._index, t._description);
					return true;
				}
				else
				{
					return false;
				}
			}
		}
	}

	//public void DebtTrack(Transaction t)
	//{
	//	int use_balance = t._balance;
	//	foreach (var old in _transaction)
	//	{
	//		if (old._debt == option.Debt.pending)
	//		{ 
	//			if (old._balance - use_balance - old._used == 0)
	//			{
	//				old._used += use_balance;
	//				t._used = t._balance;
	//				old._debt = option.Debt.cleared;
	//				return;
	//			} else if (old._balance - use_balance > 0){
	//				old._used = old._used + use_balance;
	//				t._used = use_balance;
	//				return;
	//			} else if (old._balance - use_balance < 0)
	//			{
	//				t._used += old._balance;
	//				old._used = old._balance;
	//				use_balance -= old._balance;
	//				old._debt = option.Debt.cleared;
 //               }
 //           }
	//	}
	//	t._used = t._balance - use_balance;
	//}
	public void UpdateBalance()
	{
		Database_Selection database_Selection = new Database_Selection();
		_balance = database_Selection.GetBalance(1);
	}

	public void DebtTrack(int amount, int inflowId)
	{
		Debt_Manager debt_Manager = new Debt_Manager();
		debt_Manager.Debt_Collector(amount, 1, inflowId);
	}

	public void AddTag(Transaction t)
	{
		Database_Manager dm = new Database_Manager();
		dm.AddTags(t._tags);
	}
}
