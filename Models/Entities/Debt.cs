using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class Debt
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
        public DebtType Type { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalAmount { get; set; }

        // Reduced as payments are recorded
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentBalance { get; set; }

        // Annual Percentage Rate
        [Column(TypeName = "decimal(5,2)")]
        public decimal InterestRate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MinimumPayment { get; set; }

        public DateTime? DueDate { get; set; }

        [MaxLength(200)]
        public string? Lender { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<DebtPayment> Payments { get; set; } = new List<DebtPayment>();
    }
}
