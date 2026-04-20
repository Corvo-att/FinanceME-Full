using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class Notification
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        // Null for system notifications not tied to a rule
        public int? AlertRuleId { get; set; }
        [ForeignKey(nameof(AlertRuleId))]
        public virtual AlertRule? AlertRule { get; set; }

        [Required]
        [MaxLength(300)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        [Required]
        public NotificationType Type { get; set; }

        public bool IsRead { get; set; } = false;
        public bool IsDismissed { get; set; } = false;

        // Optional deep-link to jump directly to the relevant page (e.g. "/Budgets/Detail/3")
        public string? ActionUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReadAt { get; set; }
    }
}
