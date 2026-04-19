using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;

namespace FinanceME.Models.Entities
{
    public class AlertRule
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public decimal Threshold { get; set; }
        [Required]
        public string UserId { get; set; } = string.Empty;
        public virtual User? User { get; set; }
        public virtual ICollection<Notification> GeneratedNotifications { get; set; } = new List<Notification>();

    }
}
