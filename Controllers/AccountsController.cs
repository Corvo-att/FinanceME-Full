using FinanceME.Data;
using FinanceME.Enums;
using FinanceME.Models.Entities;
using FinanceME.Models.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceME.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public AccountsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Accounts — list all active accounts for the user
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;

            var accounts = await _context.Accounts
                .Where(a => a.UserId == userId && a.IsActive)
                .OrderBy(a => a.Type)
                .ThenBy(a => a.Name)
                .ToListAsync();

            return View(accounts);
        }

        // GET: Accounts/Details/5 — account detail with recent transactions
        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User)!;

            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (account == null) return NotFound();

            var transactions = await _context.Transactions
                .Where(t => t.AccountId == id && t.UserId == userId)
                .OrderByDescending(t => t.Date)
                .Take(20)
                .Include(t => t.Category)
                .ToListAsync();

            var vm = new AccountSummaryViewModel
            {
                Account = account,
                RecentTransactions = transactions,
                TransactionCount = await _context.Transactions.CountAsync(t => t.AccountId == id && t.UserId == userId),
                TotalIncome = await _context.Transactions
                    .Where(t => t.AccountId == id && t.UserId == userId && t.Type == TransactionType.Income)
                    .SumAsync(t => (decimal?)t.Amount) ?? 0,
                TotalExpense = await _context.Transactions
                    .Where(t => t.AccountId == id && t.UserId == userId && t.Type == TransactionType.Expense)
                    .SumAsync(t => (decimal?)t.Amount) ?? 0,
            };

            return View(vm);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Account account)
        {
            // Strip any user-manipulated ID/UserId from the form — always set server-side
            ModelState.Remove(nameof(account.UserId));
            ModelState.Remove(nameof(account.User));

            if (!ModelState.IsValid) return View(account);

            account.UserId = _userManager.GetUserId(User)!;
            account.CreatedAt = DateTime.UtcNow;
            account.UpdatedAt = DateTime.UtcNow;
            account.IsActive = true;

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Account \"{account.Name}\" created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (account == null) return NotFound();
            return View(account);
        }

        // POST: Accounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Account account)
        {
            if (id != account.Id) return BadRequest();

            var userId = _userManager.GetUserId(User)!;
            var existing = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (existing == null) return NotFound();

            // Update only safe fields — never trust client-supplied UserId
            existing.Name = account.Name;
            existing.Type = account.Type;
            existing.Balance = account.Balance;
            existing.Currency = account.Currency;
            existing.Institution = account.Institution;
            existing.AccountNumber = account.AccountNumber;
            existing.Color = account.Color;
            existing.Icon = account.Icon;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Account \"{existing.Name}\" updated.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (account == null) return NotFound();

            return View(account);
        }

        // POST: Accounts/Delete/5 — soft-delete (keep transaction history intact)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (account == null) return NotFound();

            account.IsActive = false;
            account.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Account \"{account.Name}\" deactivated. Your transaction history is preserved.";
            return RedirectToAction(nameof(Index));
        }
    }
}
