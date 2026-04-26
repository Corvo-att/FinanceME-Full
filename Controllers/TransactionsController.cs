using FinanceME.Data;
using FinanceME.Enums;
using FinanceME.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FinanceME.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public TransactionsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;
            var transactions = await _context.Transactions
                .Where(t => t.UserId == userId)
                .Include(t => t.Account)
                .Include(t => t.Category)
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            return View(transactions);
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var userId = _userManager.GetUserId(User)!;
            var transaction = await _context.Transactions
                .Include(t => t.Account)
                .Include(t => t.Category)
                .Include(t => t.TransferToAccount)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (transaction == null) return NotFound();

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            SetSelectLists();
            return View();
        }

        // POST: Transactions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transaction transaction)
        {
            ModelState.Remove(nameof(transaction.UserId));
            ModelState.Remove(nameof(transaction.User));
            ModelState.Remove(nameof(transaction.Account));
            ModelState.Remove(nameof(transaction.Category));
            ModelState.Remove(nameof(transaction.TransferToAccount));

            if (ModelState.IsValid)
            {
                transaction.UserId = _userManager.GetUserId(User)!;
                transaction.CreatedAt = DateTime.UtcNow;
                transaction.UpdatedAt = DateTime.UtcNow;

                _context.Transactions.Add(transaction);
                
                await ApplyTransactionBalance(transaction);

                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Transaction created successfully.";
                return RedirectToAction(nameof(Index));
            }
            
            SetSelectLists(transaction);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userId = _userManager.GetUserId(User)!;
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            
            if (transaction == null) return NotFound();

            SetSelectLists(transaction);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Transaction transaction)
        {
            if (id != transaction.Id) return NotFound();

            ModelState.Remove(nameof(transaction.UserId));
            ModelState.Remove(nameof(transaction.User));
            ModelState.Remove(nameof(transaction.Account));
            ModelState.Remove(nameof(transaction.Category));
            ModelState.Remove(nameof(transaction.TransferToAccount));

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User)!;
                var existing = await _context.Transactions
                    .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

                if (existing == null) return NotFound();

                // Reverse the old impact before applying the new one
                await ReverseTransactionBalance(existing);

                existing.AccountId = transaction.AccountId;
                existing.CategoryId = transaction.CategoryId;
                existing.TransferToAccountId = transaction.TransferToAccountId;
                existing.Amount = transaction.Amount;
                existing.Type = transaction.Type;
                existing.Description = transaction.Description;
                existing.Notes = transaction.Notes;
                existing.Date = transaction.Date;
                existing.IsRecurring = transaction.IsRecurring;
                existing.RecurringFrequency = transaction.RecurringFrequency;
                existing.Tags = transaction.Tags;
                existing.UpdatedAt = DateTime.UtcNow;

                await ApplyTransactionBalance(existing);

                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Transaction updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            
            SetSelectLists(transaction);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userId = _userManager.GetUserId(User)!;
            var transaction = await _context.Transactions
                .Include(t => t.Account)
                .Include(t => t.Category)
                .Include(t => t.TransferToAccount)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (transaction == null) return NotFound();

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (transaction != null)
            {
                await ReverseTransactionBalance(transaction);
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Transaction deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        private void SetSelectLists(Transaction? transaction = null)
        {
            var userId = _userManager.GetUserId(User)!;
            
            ViewData["AccountId"] = new SelectList(
                _context.Accounts.Where(a => a.UserId == userId && a.IsActive),
                "Id", "Name", transaction?.AccountId);
                
            ViewData["CategoryId"] = new SelectList(
                _context.Categories.Where(c => c.UserId == userId && c.IsActive),
                "Id", "Name", transaction?.CategoryId);
                
            ViewData["TransferToAccountId"] = new SelectList(
                _context.Accounts.Where(a => a.UserId == userId && a.IsActive),
                "Id", "Name", transaction?.TransferToAccountId);
        }

        // Helpers to manage account balances
        private async Task ApplyTransactionBalance(Transaction transaction)
        {
            var account = await _context.Accounts.FindAsync(transaction.AccountId);
            if (account == null) return;

            if (transaction.Type == TransactionType.Expense)
            {
                account.Balance -= transaction.Amount;
            }
            else if (transaction.Type == TransactionType.Income)
            {
                account.Balance += transaction.Amount;
            }
            else if (transaction.Type == TransactionType.Transfer && transaction.TransferToAccountId.HasValue)
            {
                account.Balance -= transaction.Amount;
                var transferAccount = await _context.Accounts.FindAsync(transaction.TransferToAccountId);
                if (transferAccount != null)
                {
                    transferAccount.Balance += transaction.Amount;
                }
            }
        }

        private async Task ReverseTransactionBalance(Transaction transaction)
        {
            var account = await _context.Accounts.FindAsync(transaction.AccountId);
            if (account == null) return;

            if (transaction.Type == TransactionType.Expense)
            {
                account.Balance += transaction.Amount;
            }
            else if (transaction.Type == TransactionType.Income)
            {
                account.Balance -= transaction.Amount;
            }
            else if (transaction.Type == TransactionType.Transfer && transaction.TransferToAccountId.HasValue)
            {
                account.Balance += transaction.Amount;
                var transferAccount = await _context.Accounts.FindAsync(transaction.TransferToAccountId);
                if (transferAccount != null)
                {
                    transferAccount.Balance -= transaction.Amount;
                }
            }
        }
    }
}
