using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class Bill
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        public int? CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public BillFrequency Frequency { get; set; }

        // Next upcoming due date — update this after each payment
        [Required]
        public DateTime DueDate { get; set; }

        public bool IsPaid { get; set; } = false;
        public DateTime? LastPaidDate { get; set; }

        public bool AutoPay { get; set; } = false;
        public bool ReminderEnabled { get; set; } = true;
        public int ReminderDaysBefore { get; set; } = 3;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
