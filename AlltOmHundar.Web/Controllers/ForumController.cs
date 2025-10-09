using System.Linq;
using System.Threading.Tasks;
using AlltOmHundar.Core.Interfaces.Services;
using AlltOmHundar.Core.Models;
using AlltOmHundar.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AlltOmHundar.Web.Controllers
{
    public class ForumController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ITopicService _topicService;
        private readonly IPostService _postService;

        public ForumController(
            ICategoryService categoryService,
            ITopicService topicService,
            IPostService postService)
        {
            _categoryService = categoryService;
            _topicService = topicService;
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetCategoriesWithTopicsAsync();
            var safeCategories = (categories ?? Enumerable.Empty<Category>()).ToList();

            var allTopics = safeCategories
                .SelectMany(c => c.Topics ?? Enumerable.Empty<Topic>())
                .Select(t => new { Id = t.Id, Title = t.Title })
                .ToList();

            ViewBag.AllTopics = allTopics;
            return View("/Views/Forum/Index.cshtml", safeCategories);
        }

        [HttpGet]
        public async Task<IActionResult> Category(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();

            var topics = await _topicService.GetTopicsByCategoryAsync(id);
            ViewBag.Category = category;

            return View("/Views/Forum/Category.cshtml", topics);
        }

        [HttpGet]
        public async Task<IActionResult> Topic(int id)
        {
            var topic = await _topicService.GetTopicWithPostsAsync(id);
            if (topic == null) return NotFound();

            var posts = await _postService.GetTopLevelPostsByTopicAsync(id);
            ViewBag.Topic = topic;
            ViewBag.UserId = SessionHelper.GetUserId(HttpContext.Session);

            return View("/Views/Forum/Topic.cshtml", posts);
        }

        [HttpGet]
        public IActionResult NewPost(int topicId, int? parentPostId)
        {
            return RedirectToAction("CreatePost", "Post", new { topicId, parentPostId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(int topicId, string content, int? parentPostId)
        {
            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (!userId.HasValue) return RedirectToAction("Login", "Account");

            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["ErrorMessage"] = "Innehåll krävs";
                return RedirectToAction(nameof(Topic), new { id = topicId });
            }

            await _postService.CreatePostAsync(topicId, userId.Value, content, parentPostId, imageUrl: null);

            TempData["SuccessMessage"] = "Inlägg skapat!";
            return RedirectToAction(nameof(Topic), new { id = topicId });
        }
    }
}