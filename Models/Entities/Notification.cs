using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;

namespace FinanceME.Models.Entities
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        [Required]
        public string UserId { get; set; } = string.Empty;
        public virtual User? User { get; set; }
        public int? AlertRuleId {  get; set; }
        public virtual AlertRule? AlertRule { get; set; }
    }
}
