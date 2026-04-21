using FinanceME.Models.Entities;

namespace FinanceME.Models.ViewModels
{
    public class AccountSummaryViewModel
    {
        public Account Account { get; set; } = null!;
        public IList<Transaction> RecentTransactions { get; set; } = new List<Transaction>();
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal NetCashFlow => TotalIncome - TotalExpense;
        public int TransactionCount { get; set; }
    }
}
