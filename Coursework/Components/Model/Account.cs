
public class Account
{
    public int Credit(int initialBalance, int cred)
    {
        initialBalance += cred;
        return initialBalance;
    }

    public int Debit(int initialBalnce, int debt)
    {
        initialBalnce -= debt;
        return initialBalnce;
    }

    public void Debt_Track(int sundeCredit, int debt)
    {
        sundeCredit += debt;
        return;
    }
}
