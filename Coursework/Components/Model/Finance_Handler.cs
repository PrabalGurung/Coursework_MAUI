﻿using Coursework.Components.Pages;

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
		if (t._debt != null)
		{
			Database_Manager database_Manager = new Database_Manager();
			database_Manager.IncreaseBalance(1, t._balance);
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
					Database_Manager database_Manager = new Database_Manager();
					dm.InsertOutflow(1, t._balance, t._source, t._date.ToString("yyyy-MM-dd"), t._outflowType.ToString(), t._index, t._description);
					t._balance = t._balance - _balance;
					database_Manager.DecreaseBalance(1, _balance);
					dm.InsertDebt(1, t._balance, t._source, t._date.ToString("yyyy-MM-dd"), "Pending", t._index, t._description);
					return false;
				}
			}
		}
	}

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
