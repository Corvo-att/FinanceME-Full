using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;

namespace FinanceME.Models.Entities
{
    public class Budget
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public BudgetPeriod Period { get; set; }
        public decimal Threshold { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public virtual User? User { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}