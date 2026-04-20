using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class Forecast
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Always store as the first day of the target month (e.g. 2025-06-01)
        [Required]
        public DateTime ForecastMonth { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ProjectedIncome { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ProjectedExpenses { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ProjectedSavings { get; set; }

        // Filled in once the month closes — used for the projected vs actual comparison chart
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ActualIncome { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ActualExpenses { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ActualSavings { get; set; }

        public string? Assumptions { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
