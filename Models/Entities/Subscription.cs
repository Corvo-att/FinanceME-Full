using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;

namespace FinanceME.Models.Entities
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; } 
        public DateTime RenewalDate { get; set; }
        public SubscriptionStatus Status { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public virtual User? User { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }

    }
}
