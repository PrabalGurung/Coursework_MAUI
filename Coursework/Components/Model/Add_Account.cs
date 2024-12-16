using Coursework.Components.Pages;

public class Add_Account
{
	public string _name { get; set; }
	public int _balance { get; set; }
	public List<Transaction> _transaction { get; set; } = new List<Transaction>();

	public Add_Account()
	{
	}

	public Add_Account(string name, int balance)
	{
		_name = name;
		_balance = balance;
	}

	public bool History(Transaction t)
	{
		option.Cash_Outflow _type = option.Cash_Outflow.choose;
		option.Debt debt = option.Debt.choose;

		if (t._debt != debt)
		{
			_transaction.Add(t);
			return true;
		}
		else
		{
			if (t._outflowType == _type)
			{
				DebtTrack(t);
				_balance += t._balance - t._used;
				_transaction.Add(t);
				return true;
			}
			else
			{
				if (_balance > t._balance)
				{
					_balance -= t._balance;
					_transaction.Add(t);
					return true;
				}
				else
				{
					return false;
				}
			}
		}
	}

	public void DebtTrack(Transaction t)
	{
		int use_balance = t._balance;
		foreach (var old in _transaction)
		{
			if (old._debt == option.Debt.pending)
			{ 
				if (old._balance - use_balance - old._used == 0)
				{
					old._used += use_balance;
					t._used = t._balance;
					old._debt = option.Debt.cleared;
					return;
				} else if (old._balance - use_balance > 0){
					old._used = old._used + use_balance;
					t._used = use_balance;
					return;
				} else if (old._balance - use_balance < 0)
				{
					t._used += old._balance;
					old._used = old._balance;
					use_balance -= old._balance;
					old._debt = option.Debt.cleared;
                }
            }
		}
		t._used = t._balance - use_balance;
	}
}
