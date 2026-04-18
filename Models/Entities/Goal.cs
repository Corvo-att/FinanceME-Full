using System.ComponentModel.DataAnnotations;

namespace FinanceME.Models.Entities
{
    public class Goal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal CurrentSaved { get; set; }

        [Required]
        public decimal TargetAmount { get; set; }

        [Required]
        public decimal MonthlyContribution { get; set; }

        [Required]
        public DateTime TargetDate { get; set; }


        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public string Priority {  get; set; } = string.Empty;

        public string? Notes {  get; set; }
    }
}
