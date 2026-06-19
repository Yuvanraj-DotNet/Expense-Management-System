using Microsoft.EntityFrameworkCore;
using Expense_Management_System.Models;

namespace Expense_Management_System.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseApproval> ExpenseApprovals { get; set; }
        public DbSet<Reimbursement> Reimbursements { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ExpenseCategory>()
                .Property(e => e.MaxAllowedAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Reimbursement>()
                .Property(r => r.Amount)
                .HasPrecision(18, 2);
        }
    }
}