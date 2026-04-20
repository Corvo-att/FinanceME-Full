using FinanceME.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinanceME.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account>          Accounts          { get; set; }
        public DbSet<Category>         Categories        { get; set; }
        public DbSet<Transaction>      Transactions      { get; set; }
        public DbSet<Budget>           Budgets           { get; set; }
        public DbSet<Goal>             Goals             { get; set; }
        public DbSet<GoalContribution> GoalContributions { get; set; }
        public DbSet<Bill>             Bills             { get; set; }
        public DbSet<Forecast>         Forecasts         { get; set; }
        public DbSet<Debt>             Debts             { get; set; }
        public DbSet<DebtPayment>      DebtPayments      { get; set; }
        public DbSet<Subscription>     Subscriptions     { get; set; }
        public DbSet<Notification>     Notifications     { get; set; }
        public DbSet<AlertRule>        AlertRules        { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Must call base first — this sets up all the Identity tables
            base.OnModelCreating(builder);

            ConfigureAccount(builder);
            ConfigureCategory(builder);
            ConfigureTransaction(builder);
            ConfigureBudget(builder);
            ConfigureGoal(builder);
            ConfigureBill(builder);
            ConfigureForecast(builder);
            ConfigureDebt(builder);
            ConfigureSubscription(builder);
            ConfigureNotification(builder);
            ConfigureAlertRule(builder);
        }

        private static void ConfigureAccount(ModelBuilder builder)
        {
            builder.Entity<Account>(entity =>
            {
                entity.HasOne(a => a.User)
                    .WithMany(u => u.Accounts)
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(a => new { a.UserId, a.IsActive });
            });
        }

        private static void ConfigureCategory(ModelBuilder builder)
        {
            builder.Entity<Category>(entity =>
            {
                entity.HasOne(c => c.User)
                    .WithMany(u => u.Categories)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Restrict prevents deleting a parent category that still has children
                entity.HasOne(c => c.ParentCategory)
                    .WithMany(c => c.SubCategories)
                    .HasForeignKey(c => c.ParentCategoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                entity.HasIndex(c => new { c.UserId, c.Name });
            });
        }

        private static void ConfigureTransaction(ModelBuilder builder)
        {
            builder.Entity<Transaction>(entity =>
            {
                // Restrict so deleting an account doesn't silently wipe its transaction history
                entity.HasOne(t => t.Account)
                    .WithMany(a => a.Transactions)
                    .HasForeignKey(t => t.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);

                // If the destination account is deleted, just null out the reference
                entity.HasOne(t => t.TransferToAccount)
                    .WithMany()
                    .HasForeignKey(t => t.TransferToAccountId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);

                
                entity.HasOne(t => t.Category)
                    .WithMany(c => c.Transactions)
                    .HasForeignKey(t => t.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .IsRequired(false);

                
                entity.HasOne(t => t.User)
                    .WithMany(u => u.Transactions)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasIndex(t => t.Date);
                entity.HasIndex(t => new { t.UserId, t.Date });
                entity.HasIndex(t => new { t.UserId, t.CategoryId });
                entity.HasIndex(t => new { t.UserId, t.AccountId });
            });
        }

        private static void ConfigureBudget(ModelBuilder builder)
        {
            builder.Entity<Budget>(entity =>
            {
                entity.HasOne(b => b.User)
                    .WithMany(u => u.Budgets)
                    .HasForeignKey(b => b.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Restrict so we can't accidentally delete a category that budgets depend on
                entity.HasOne(b => b.Category)
                    .WithMany(c => c.Budgets)
                    .HasForeignKey(b => b.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(b => new { b.UserId, b.IsActive });
                entity.HasIndex(b => new { b.UserId, b.CategoryId });
            });
        }

        private static void ConfigureGoal(ModelBuilder builder)
        {
            builder.Entity<Goal>(entity =>
            {
                entity.HasOne(g => g.User)
                    .WithMany(u => u.Goals)
                    .HasForeignKey(g => g.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(g => new { g.UserId, g.Status });
            });

            builder.Entity<GoalContribution>(entity =>
            {
                entity.HasOne(gc => gc.Goal)
                    .WithMany(g => g.Contributions)
                    .HasForeignKey(gc => gc.GoalId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(gc => new { gc.GoalId, gc.Date });
            });
        }

        private static void ConfigureBill(ModelBuilder builder)
        {
            builder.Entity<Bill>(entity =>
            {
                // ClientCascade: avoids the multi-path conflict User→Category→Bill
                entity.HasOne(b => b.User)
                    .WithMany(u => u.Bills)
                    .HasForeignKey(b => b.UserId)
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne(b => b.Category)
                    .WithMany()
                    .HasForeignKey(b => b.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .IsRequired(false);

                entity.HasIndex(b => new { b.UserId, b.DueDate });
                entity.HasIndex(b => new { b.UserId, b.IsActive });
            });
        }

        private static void ConfigureForecast(ModelBuilder builder)
        {
            builder.Entity<Forecast>(entity =>
            {
                entity.HasOne(f => f.User)
                    .WithMany(u => u.Forecasts)
                    .HasForeignKey(f => f.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                // A user can only have one forecast per calendar month
                entity.HasIndex(f => new { f.UserId, f.ForecastMonth }).IsUnique();
            });
        }

        private static void ConfigureDebt(ModelBuilder builder)
        {
            builder.Entity<Debt>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(u => u.Debts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(d => new { d.UserId, d.IsActive });
            });

            builder.Entity<DebtPayment>(entity =>
            {
                entity.HasOne(dp => dp.Debt)
                    .WithMany(d => d.Payments)
                    .HasForeignKey(dp => dp.DebtId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(dp => new { dp.DebtId, dp.Date });
            });
        }

        private static void ConfigureSubscription(ModelBuilder builder)
        {
            builder.Entity<Subscription>(entity =>
            {
                // ClientCascade: avoids the multi-path conflict User→Category→Subscription
                entity.HasOne(s => s.User)
                    .WithMany(u => u.Subscriptions)
                    .HasForeignKey(s => s.UserId)
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne(s => s.Category)
                    .WithMany()
                    .HasForeignKey(s => s.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .IsRequired(false);

                entity.HasIndex(s => new { s.UserId, s.Status });
            });
        }

        private static void ConfigureNotification(ModelBuilder builder)
        {
            builder.Entity<Notification>(entity =>
            {
                // ClientCascade: avoids the multi-path conflict User→AlertRule→Notification
                entity.HasOne(n => n.User)
                    .WithMany(u => u.Notifications)
                    .HasForeignKey(n => n.UserId)
                    .OnDelete(DeleteBehavior.ClientCascade);

                // Keep the notification even if the rule that triggered it is deleted
                entity.HasOne(n => n.AlertRule)
                    .WithMany(ar => ar.GeneratedNotifications)
                    .HasForeignKey(n => n.AlertRuleId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);

                entity.HasIndex(n => new { n.UserId, n.IsRead });
                entity.HasIndex(n => new { n.UserId, n.CreatedAt });
            });
        }

        private static void ConfigureAlertRule(ModelBuilder builder)
        {
            builder.Entity<AlertRule>(entity =>
            {
                entity.HasOne(ar => ar.User)
                    .WithMany(u => u.AlertRules)
                    .HasForeignKey(ar => ar.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(ar => new { ar.UserId, ar.IsEnabled });
            });
        }
    }
}
