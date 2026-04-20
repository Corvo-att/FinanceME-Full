using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FinanceME.Models.Entities
{
    // Extends IdentityUser which already handles Id, Email, PasswordHash, TwoFactor, Lockout, etc.
    public class User : IdentityUser
    {
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(10)]
        public string PreferredCurrency { get; set; } = "EGP";

        [MaxLength(50)]
        public string Locale { get; set; } = "ar-EG";

        [MaxLength(100)]
        public string Timezone { get; set; } = "UTC";

        public string? ProfilePictureUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
        public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();
        public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();
        public virtual ICollection<Forecast> Forecasts { get; set; } = new List<Forecast>();
        public virtual ICollection<Debt> Debts { get; set; } = new List<Debt>();
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<AlertRule> AlertRules { get; set; } = new List<AlertRule>();
    }
}