using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required]
        public int AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; } = null!;

        public int? CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }

        // Only set when Type == Transfer
        public int? TransferToAccountId { get; set; }
        [ForeignKey(nameof(TransferToAccountId))]
        public virtual Account? TransferToAccount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public TransactionType Type { get; set; }

        [Required]
        [MaxLength(300)]
        public string Description { get; set; } = string.Empty;

        public string? Notes { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public bool IsRecurring { get; set; } = false;
        public BillFrequency? RecurringFrequency { get; set; }

        // Points back to the template transaction that generated this recurring instance
        public int? RecurringSourceId { get; set; }

        public bool IsAutoCategorized { get; set; } = false;

        // Comma-separated tags e.g. "vacation,business"
        [MaxLength(500)]
        public string? Tags { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}