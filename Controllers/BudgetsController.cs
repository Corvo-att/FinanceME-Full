using FinanceME.Data;
using FinanceME.Enums;
using FinanceME.Models.Entities;
using FinanceME.Models.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FinanceME.Controllers
{
    [Authorize]
    public class BudgetsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public BudgetsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {

            var userId = _userManager.GetUserId(User)!;

            var details = _context.Budgets
                       .Include(b => b.Category)
                       .Where(b => b.UserId == userId)
                       .ToList();

            // Calculate spent amount per budget from actual transactions
            foreach (var b in details)
            {
                b.SpentAmount = _context.Transactions
                    .Where(t => t.UserId == userId && t.CategoryId == b.CategoryId 
                             && t.Date >= b.StartDate && t.Date <= b.EndDate)
                    .Sum(t => (decimal?)t.Amount) ?? 0;
            }

            // Calculate dynamic totals for KPIs
            ViewBag.TotalBudget = details.Sum(b => b.LimitAmount);
            ViewBag.TotalSpent = details.Sum(b => b.SpentAmount);
            ViewBag.Remaining = ViewBag.TotalBudget - ViewBag.TotalSpent;

            // CHANGE: Determine which budgets have triggered the 80% alert
            ViewBag.AlertBudgetIds = details
                .Where(b => b.AlertOnThreshold && b.SpentAmount >= b.LimitAmount * (b.WarningThreshold / 100))
                .Select(b => b.Id)
                .ToHashSet();

            return View(details);
        }

        public IActionResult Budgetdetail(int id)
        {
            var userId = _userManager.GetUserId(User)!;

            var details = _context.Budgets
                           .Include(b => b.Category)
                           .FirstOrDefault(x => x.Id == id);


            Transactionlist(details);

            return View(details);
        }



        [HttpPost]
        public IActionResult Savebudget(Budget budget)
        {

            // We remove these because we set them manually below, not from the form
            ModelState.Remove(nameof(budget.UserId));
            ModelState.Remove(nameof(budget.User));
            ModelState.Remove(nameof(budget.Category));



            var userId = _userManager.GetUserId(User)!;
            budget.UserId = userId;

            // FIX: Check validation after removing fields that are handled manually
            if (!ModelState.IsValid)
            {
                SetSelectLists(budget);
                return View("Createbudget", budget);
            }

            var existingBudget = _context.Budgets.FirstOrDefault(b => b.Id == budget.Id && b.UserId == userId);

            if (existingBudget != null)
            {
                existingBudget.LimitAmount = budget.LimitAmount;
                existingBudget.CategoryId = budget.CategoryId;
                existingBudget.Period = budget.Period;
                existingBudget.EndDate = budget.EndDate;
                // CHANGE: Persist the rollover and alert preferences
                existingBudget.RolloverUnused = budget.RolloverUnused;
                existingBudget.AlertOnThreshold = budget.AlertOnThreshold;
                existingBudget.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                budget.CreatedAt = DateTime.UtcNow;
                budget.StartDate = DateTime.UtcNow;
                budget.IsActive = true;
                _context.Budgets.Add(budget);
            }

            switch (budget.Period)
            {
                case BudgetPeriod.Weekly:
                    budget.EndDate = budget.StartDate.AddDays(7);
                    break;

                case BudgetPeriod.Yearly:
                    budget.EndDate = budget.StartDate.AddYears(1);
                    break;

                case BudgetPeriod.Monthly:
                default:

                    budget.EndDate = budget.StartDate.AddMonths(1);
                    break;
            }


            _context.SaveChanges();

            return RedirectToAction("Budgetdetail", new { id = budget.Id });


        }

        public IActionResult Editbudget(int? id)
        {
            var userId = _userManager.GetUserId(User)!;
            var budget = _context.Budgets.FirstOrDefault(b => b.Id == id && b.UserId == userId);

            SetSelectLists(budget);



            return View("Createbudget", budget);
        }


        public IActionResult Createbudget()
        {
            SetSelectLists();
            return View("Createbudget");
        }



        private void SetSelectLists(Budget? budget = null)
        {
            var userId = _userManager.GetUserId(User)!;

            ViewBag.CategoryId = new SelectList(
                _context.Categories.Where(c => c.UserId == userId && c.IsActive),
                "Id", "Name", budget?.CategoryId);

        }





        private void Transactionlist(Budget? budget = null)
        {
            var userId = _userManager.GetUserId(User)!;

            ViewBag.Transactions = _context.Transactions.Include(t => t.Category).Where(t => t.UserId == userId && t.CategoryId == budget.CategoryId).ToList();

        }


        public IActionResult DeleteBudget(int? id)
        {
            var del = _context.Budgets.FirstOrDefault(b => b.Id == id);

            _context.Budgets.Remove(del);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }




    }
}
