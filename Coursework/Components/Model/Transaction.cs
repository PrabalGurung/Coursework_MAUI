
using System.Security.Cryptography.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Transaction
{
    public int _balance { get; set; }
    public option.Cash_Inflow _inflowType { get; set; }
	public option.Cash_Outflow _outflowType { get; set; }
	public string _debt { get; set; }
	public string _source { get; set; }
	public DateOnly _date { get; set; }
	public int _used { get; set; }

	public Transaction() { }

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

    public Transaction(int balance, string debt, string source, DateOnly date)
    {
        _balance = balance;
        _debt = debt;
        _date = date;
        _source = source;
    }
}