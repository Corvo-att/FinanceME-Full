using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceME.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Icon { get; set; }

        [MaxLength(20)]
        public string? Color { get; set; }

        // Supports one level of parent -> child nesting (e.g. "Food" -> "Groceries")
        public int? ParentCategoryId { get; set; }
        [ForeignKey(nameof(ParentCategoryId))]
        public virtual Category? ParentCategory { get; set; }
        public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>();

        // True for built-in system categories, false for user-created ones
        public bool IsDefault { get; set; } = false;
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
    }
}