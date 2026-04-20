using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class Subscription
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        public int? CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public BillFrequency BillingCycle { get; set; }

        [Required]
        public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;

        public DateTime StartDate { get; set; }
        public DateTime? NextBillingDate { get; set; }
        public DateTime? CancellationDate { get; set; }

        [MaxLength(200)]
        public string? Provider { get; set; }

        public string? LogoUrl { get; set; }

        public bool ReminderEnabled { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
