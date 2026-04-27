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
            return View();
        }

        public IActionResult Budgetdetail()
        {
            return View();
        }

        public IActionResult Createbudget()
        {
            return View();
        }

        public async Task<IActionResult> Savebudget(Budget budget)
        {
            ModelState.Remove(nameof(budget.UserId));
            ModelState.Remove(nameof(budget.User));

            if (!ModelState.IsValid) return View(budget);

            budget.UserId = _userManager.GetUserId(User)!;
            budget.CreatedAt = DateTime.UtcNow;
            budget.UpdatedAt = DateTime.UtcNow;
            budget.IsActive = true;

            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }
    }
}
