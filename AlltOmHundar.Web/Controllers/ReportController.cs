using AlltOmHundar.Core.Interfaces.Services;
using AlltOmHundar.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlltOmHundar.Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IPostService _postService;

        public ReportController(IReportService reportService, IPostService postService)
        {
            _reportService = reportService;
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int postId, int topicId)
        {
            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            // Använd GetPostWithRepliesAsync istället för att få med User
            var post = await _postService.GetPostWithRepliesAsync(postId);
            if (post == null)
                return NotFound();

            ViewBag.Post = post;
            ViewBag.TopicId = topicId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int postId, int topicId, string reason)
        {
            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            if (string.IsNullOrWhiteSpace(reason))
            {
                TempData["ErrorMessage"] = "Du måste ange en anledning";
                return RedirectToAction("Create", new { postId, topicId });
            }

            await _reportService.CreateReportAsync(postId, userId.Value, reason);

            TempData["SuccessMessage"] = "Tack för din anmälan! En administratör kommer granska inlägget.";
            return RedirectToAction("Topic", "Forum", new { id = topicId });
        }
    }
}