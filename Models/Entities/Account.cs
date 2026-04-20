using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class Account
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public AccountType Type { get; set; }

        // Positive for assets (checking/savings), negative for liabilities (credit cards)
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        [MaxLength(10)]
        public string Currency { get; set; } = "EGP";

        [MaxLength(200)]
        public string? Institution { get; set; }

        // Store only the last 4 digits
        [MaxLength(20)]
        public string? AccountNumber { get; set; }

        public string? Color { get; set; }
        public string? Icon { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}