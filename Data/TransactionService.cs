using BudgetBuddy.Models;

namespace BudgetBuddy.Data;

public interface ITransactionService
{
    void Add(Transaction transaction);
    void Remove(Transaction transaction);
    IEnumerable<Transaction> GetAll();
    decimal GetTotalByType(TransactionType type);
    string GetSummary();
}

public class TransactionService : ITransactionService
{
    private readonly List<Transaction> _transactions = [];

    public void Add(Transaction transaction) => _transactions.Add(transaction);

    public void Remove(Transaction transaction) => _transactions.Remove(transaction);

    public IEnumerable<Transaction> GetAll()
        => _transactions.OrderByDescending(t => t.Date);

    public decimal GetTotalByType(TransactionType type)
        => _transactions
            .Where(t => t.Type == type)
            .Sum(t => t.Amount);

    public string GetSummary()
    {
        var totalIncome = GetTotalByType(TransactionType.Income);
        var totalExpense = GetTotalByType(TransactionType.Expense);
        var balance = totalIncome - totalExpense;

        return balance switch
        {
            > 0 => $"Masz nadwyżkę w wysokości {balance:C}.",
            < 0 => $"Masz deficyt w wysokości {-balance:C}.",
            _ => "Twój budżet jest zrównoważony."
        };
    }
}

