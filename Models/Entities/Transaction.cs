using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;

namespace FinanceME.Models.Entities
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public TransactionType Type { get; set; }

        // Recurring fields
        public bool IsRecurring { get; set; }
        public BillFrequency? RecurringFrequency { get; set; }

        // Tags field
        public string? Tags { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public virtual User? User { get; set; }

        [Required]
        public int AccountId { get; set; }
        public virtual Account? Account { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        public int? TransferToAccountId { get; set; }
        public virtual Account? TransferToAccount { get; set; }
    }
}