using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class GoalContribution
    {
        public int Id { get; set; }

        [Required]
        public int GoalId { get; set; }
        [ForeignKey(nameof(GoalId))]
        public virtual Goal Goal { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
