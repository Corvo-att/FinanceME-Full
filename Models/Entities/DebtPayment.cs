using FinanceME.Enums;
using System.ComponentModel.DataAnnotations;

namespace FinanceME.Models.Entities
{
    public class Debtpayments
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool IsExtraPayment { get; set; }

        [Required]

        public int DebtId { get; set; }
        public virtual Dept? Debt { get; set; }
         


    }
}
