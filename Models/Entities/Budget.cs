using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class Budget
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required(ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }


        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal LimitAmount { get; set; }

        // Cached sum of transactions — updated on every transaction write to avoid slow dashboard queries
        [Column(TypeName = "decimal(18,2)")]
        public decimal SpentAmount { get; set; } = 0;

        [Required]
        public BudgetPeriod Period { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Percentage (0-100) at which a "near limit" alert fires — default is 80%
        [Column(TypeName = "decimal(5,2)")]
        public decimal WarningThreshold { get; set; } = 80.00m;

        public bool IsActive { get; set; } = true;

        // CHANGE: Carry unused budget forward to the next period when true
        public bool RolloverUnused { get; set; } = false;

        // CHANGE: Show in-app alert when spending reaches WarningThreshold %
        public bool AlertOnThreshold { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}