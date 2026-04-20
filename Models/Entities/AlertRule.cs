using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class AlertRule
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public AlertCondition Condition { get; set; }

        // Meaning depends on Condition: dollar amount for LowBalance/LargeTransaction,
        // percentage for BudgetNearLimit/GoalMilestone
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ThresholdValue { get; set; }

        // Optional — limit this rule to a specific account, budget, or goal
        public int? AccountId { get; set; }
        public int? BudgetId { get; set; }
        public int? GoalId { get; set; }

        public bool IsEnabled { get; set; } = true;
        public bool NotifyByEmail { get; set; } = false;
        public bool NotifyInApp { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Notification> GeneratedNotifications { get; set; } = new List<Notification>();
    }
}
