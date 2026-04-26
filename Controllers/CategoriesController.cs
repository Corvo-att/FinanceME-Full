using FinanceME.Data;
using FinanceME.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FinanceME.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CategoriesController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;
            var categories = await _context.Categories
                .Where(c => c.UserId == userId && c.ParentCategoryId == null && c.IsActive)
                .Include(c => c.SubCategories.Where(sc => sc.IsActive))
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(categories);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var userId = _userManager.GetUserId(User)!;
            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories.Where(sc => sc.IsActive))
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (category == null) return NotFound();

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            SetSelectLists();
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            ModelState.Remove(nameof(category.UserId));
            ModelState.Remove(nameof(category.User));
            ModelState.Remove(nameof(category.ParentCategory));
            ModelState.Remove(nameof(category.SubCategories));
            ModelState.Remove(nameof(category.Transactions));
            ModelState.Remove(nameof(category.Budgets));

            if (ModelState.IsValid)
            {
                category.UserId = _userManager.GetUserId(User)!;
                category.CreatedAt = DateTime.UtcNow;
                category.IsActive = true;
                category.IsDefault = false;

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Category created successfully.";
                return RedirectToAction(nameof(Index));
            }
            SetSelectLists(category);
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userId = _userManager.GetUserId(User)!;
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
            
            if (category == null) return NotFound();

            SetSelectLists(category);
            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id) return NotFound();

            ModelState.Remove(nameof(category.UserId));
            ModelState.Remove(nameof(category.User));
            ModelState.Remove(nameof(category.ParentCategory));
            ModelState.Remove(nameof(category.SubCategories));
            ModelState.Remove(nameof(category.Transactions));
            ModelState.Remove(nameof(category.Budgets));

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User)!;
                var existing = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

                if (existing == null) return NotFound();

                if (existing.IsDefault)
                {
                    ModelState.AddModelError("", "Cannot edit default system categories.");
                    SetSelectLists(category);
                    return View(category);
                }

                existing.Name = category.Name;
                existing.Icon = category.Icon;
                existing.Color = category.Color;
                existing.ParentCategoryId = category.ParentCategoryId;

                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Category updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            SetSelectLists(category);
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userId = _userManager.GetUserId(User)!;
            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (category == null) return NotFound();
            
            var inUse = await _context.Transactions.AnyAsync(t => t.CategoryId == id) ||
                        await _context.Budgets.AnyAsync(b => b.CategoryId == id);
            ViewBag.IsInUse = inUse;

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (category == null) return NotFound();

            if (category.IsDefault)
            {
                TempData["ErrorMessage"] = "Cannot delete default system categories.";
                return RedirectToAction(nameof(Index));
            }

            var inUse = await _context.Transactions.AnyAsync(t => t.CategoryId == id) ||
                        await _context.Budgets.AnyAsync(b => b.CategoryId == id);

            if (inUse)
            {
                ModelState.AddModelError("", "Cannot delete: category is in use by transactions or budgets.");
                ViewBag.IsInUse = true;
                return View("Delete", category);
            }

            // Soft delete
            category.IsActive = false;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Category deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private void SetSelectLists(Category? category = null)
        {
            var userId = _userManager.GetUserId(User)!;
            var parents = _context.Categories.Where(c => c.UserId == userId && c.ParentCategoryId == null && c.IsActive);
            
            // Exclude current category to prevent self-loop
            if (category != null && category.Id > 0)
            {
                parents = parents.Where(c => c.Id != category.Id);
            }

            ViewData["ParentCategoryId"] = new SelectList(parents, "Id", "Name", category?.ParentCategoryId);
        }
    }
}
