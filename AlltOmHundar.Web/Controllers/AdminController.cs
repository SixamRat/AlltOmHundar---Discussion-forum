using AlltOmHundar.Core.Interfaces.Services;
using AlltOmHundar.Core.Enums;
using AlltOmHundar.Web.Helpers;
using AlltOmHundar.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlltOmHundar.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ITopicService _topicService;
        private readonly IReportService _reportService;
        private readonly IUserService _userService;

        public AdminController(
            ICategoryService categoryService,
            ITopicService topicService,
            IReportService reportService,
            IUserService userService)
        {
            _categoryService = categoryService;
            _topicService = topicService;
            _reportService = reportService;
            _userService = userService;
        }

        // Kontrollera att användaren är admin
        private bool IsAdmin()
        {
            return SessionHelper.IsAdmin(HttpContext.Session);
        }

        // Admin Dashboard
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var pendingReports = await _reportService.GetPendingReportsAsync();
            ViewBag.PendingReportsCount = pendingReports.Count();

            return View();
        }

        // === KATEGORI-HANTERING ===

        [HttpGet]
        public async Task<IActionResult> Categories()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var categories = await _categoryService.GetAllCategoriesAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryViewModel model)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(model);

            await _categoryService.CreateCategoryAsync(model.Name, model.Description, model.DisplayOrder);

            TempData["SuccessMessage"] = "Kategori skapad!";
            return RedirectToAction("Categories");
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();

            var model = new EditCategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                DisplayOrder = category.DisplayOrder
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(EditCategoryViewModel model)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(model);

            var success = await _categoryService.UpdateCategoryAsync(
                model.Id,
                model.Name,
                model.Description,
                model.DisplayOrder);

            if (!success)
            {
                TempData["ErrorMessage"] = "Kunde inte uppdatera kategori";
                return View(model);
            }

            TempData["SuccessMessage"] = "Kategori uppdaterad!";
            return RedirectToAction("Categories");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var success = await _categoryService.DeleteCategoryAsync(id);

            if (success)
                TempData["SuccessMessage"] = "Kategori borttagen!";
            else
                TempData["ErrorMessage"] = "Kunde inte ta bort kategori";

            return RedirectToAction("Categories");
        }

        // === ÄMNE-HANTERING ===

        [HttpGet]
        public async Task<IActionResult> CreateTopic(int categoryId)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var category = await _categoryService.GetCategoryByIdAsync(categoryId);
            if (category == null)
                return NotFound();

            ViewBag.Category = category;
            return View(new CreateTopicViewModel { CategoryId = categoryId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateTopic(CreateTopicViewModel model)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                var category = await _categoryService.GetCategoryByIdAsync(model.CategoryId);
                ViewBag.Category = category;
                return View(model);
            }

            await _topicService.CreateTopicAsync(model.CategoryId, model.Title, model.Description);

            TempData["SuccessMessage"] = "Ämne skapat!";
            return RedirectToAction("Category", "Forum", new { id = model.CategoryId });
        }

        // === RAPPORT-HANTERING ===

        [HttpGet]
        public async Task<IActionResult> Reports()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var reports = await _reportService.GetPendingReportsAsync();
            return View(reports);
        }

        [HttpPost]
        public async Task<IActionResult> ResolveReport(int reportId, string action, string? adminNotes)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            ReportStatus status = action == "dismiss" ? ReportStatus.Dismissed : ReportStatus.Resolved;

            await _reportService.UpdateReportStatusAsync(reportId, status, userId.Value, adminNotes);

            TempData["SuccessMessage"] = "Rapport hanterad!";
            return RedirectToAction("Reports");
        }
    }
}