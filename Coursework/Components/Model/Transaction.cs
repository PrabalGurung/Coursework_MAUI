
using System.Security.Cryptography.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Transaction
{
    public int _balance;
    public option.Cash_Inflow _inflowType;
    public option.Cash_Outflow _outflowType;
    public option.Debt _debt;
    public string _source;
    public DateOnly _date;
    public int _used;

    public Transaction(int balance, option.Cash_Inflow type ,string source, DateOnly date)
    {
        _balance = balance;
		_inflowType = type;
        _source = source;
        _date = date;
    }

	public Transaction(int balance, option.Cash_Outflow type, string source, DateOnly date)
	{
		_balance = balance;
		_outflowType = type;
		_source = source;
		_date = date;
	}

    public Transaction(int balance, option.Debt debt, string source, DateOnly date)
    {
        _balance = balance;
        _debt = debt;
        _date = date;
        _source = source;
    }
}

