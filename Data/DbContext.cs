using Microsoft.EntityFrameworkCore;
using BudgetBuddy.Models;

namespace BudgetBuddy.Data
{
    public class BudgetDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public BudgetDbContext(DbContextOptions<BudgetDbContext> options)
            : base(options)
        {
        }

        // Add your DbSet properties here, for example:
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Add your model configurations here

            modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasPrecision(18, 2);
        }
    }
}