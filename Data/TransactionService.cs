using System.Threading.Tasks;
using BudgetBuddy.Models;

using Microsoft.EntityFrameworkCore;
namespace BudgetBuddy.Data;

public interface ITransactionService
{
    Task AddAsync(Transaction transaction);
    Task RemoveAsync(Transaction transaction);
    Task<IEnumerable<Transaction>> GetAllAsync();
    Task<decimal> GetTotalByTypeAsync(TransactionType type);
    Task<string> GetSummaryAsync();
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

    public async Task<IEnumerable<Transaction>> GetAllAsync()
        => await _context.Set<Transaction>().OrderByDescending(t => t.Date).ToListAsync();

    public async Task<decimal> GetTotalByTypeAsync(TransactionType type)
        => await _context.Set<Transaction>()
            .Where(t => t.Type == type)
            .SumAsync(t => t.Amount);

    public async Task<string> GetSummaryAsync()
    {
        var totalIncome = await GetTotalByTypeAsync(TransactionType.Income);
        var totalExpense = await GetTotalByTypeAsync(TransactionType.Expense);
        var balance = totalIncome - totalExpense;

        return balance switch
        {
            > 0 => $"Masz nadwyżkę w wysokości {balance:C}.",
            < 0 => $"Masz deficyt w wysokości {-balance:C}.",
            _ => "Twój budżet jest zrównoważony."
        };
    }
}

