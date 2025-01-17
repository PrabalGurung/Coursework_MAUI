
using System.Security.Cryptography.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;
/* --
 * Model for transaction
 -- */
public class Transaction
{
	//Declaration
    public int _balance { get; set; }
    public option.Cash_Inflow _inflowType { get; set; }
	public option.Cash_Outflow _outflowType { get; set; }
	public string _debt { get; set; }
	public string _source { get; set; }
	public DateOnly _date { get; set; }
	public int _used { get; set; }
	public string _description { get; set; }
	public string _tags { get; set; }
	public int _index {  get; set; }

	// No parameterized constructor 
	public Transaction() { }

	// Parameterized constructor || Override
	public Transaction(int balance, option.Cash_Inflow type ,string source, DateOnly date, int tags, string _notes)
    {
        _balance = balance;
		_inflowType = type;
        _source = source;
        _date = date;
		_index = tags;
		_description = _notes;
    }

	// Parameterized constructor || Override
	public Transaction(int balance, option.Cash_Outflow type, string source, DateOnly date, int tags, string _notes)
	{
		_balance = balance;
		_outflowType = type;
		_source = source;
		_date = date;
		_index = tags;
		_description = _notes;
	}

	// Parameterized constructor || Override
	public Transaction(int balance, string debt, string source, DateOnly date, int tags, string notes)
    {
        _balance = balance;
        _debt = debt;
        _date = date;
        _source = source;
		_index = tags;
		_description = notes;
    }

	// Parameterized constructor
	public Transaction(string tags)
	{
		_tags = tags;
	}

}