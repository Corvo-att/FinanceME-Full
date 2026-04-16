using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;

namespace FinanceME.Models.Entities
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public AccountType Type { get; set; }
        public decimal Balance { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public virtual User? User { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}