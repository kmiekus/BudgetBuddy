using BudgetBuddy.Models;

using Microsoft.EntityFrameworkCore;
namespace BudgetBuddy.Data;

public interface ITransactionService
{
    Task AddAsync(Transaction transaction);
    Task RemoveAsync(Transaction transaction);
    Task<IEnumerable<Transaction>> GetAll();
    decimal GetTotalByType(TransactionType type);
    string GetSummary();
}

public class TransactionService : ITransactionService
{
    private readonly BudgetDbContext _context;

    public TransactionService(BudgetDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Transaction transaction)
    {
        await _context.Set<Transaction>().AddAsync(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Transaction transaction) => await RemoveAsync(transaction.Id);
    public async Task RemoveAsync(Guid id)
    {
        var transaction = await _context.Set<Transaction>().FindAsync(id);
        if (transaction != null)
        {
            _context.Set<Transaction>().Remove(transaction);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Transaction>> GetAll()
        => await _context.Set<Transaction>().OrderByDescending(t => t.Date).ToListAsync();

    public decimal GetTotalByType(TransactionType type)
        => _context.Set<Transaction>()
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

