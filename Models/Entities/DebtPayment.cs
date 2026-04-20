using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class DebtPayment
    {
        public int Id { get; set; }

        [Required]
        public int DebtId { get; set; }
        [ForeignKey(nameof(DebtId))]
        public virtual Debt Debt { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrincipalPortion { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal InterestPortion { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
