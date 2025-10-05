using AlltOmHundar.Core.Interfaces.Services;
using AlltOmHundar.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        // Visa alla kategorier
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetCategoriesWithTopicsAsync();
            return View(categories);
        }

        // Visa ämnen i en kategori
        public async Task<IActionResult> Category(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();

            var topics = await _topicService.GetTopicsByCategoryAsync(id);
            ViewBag.Category = category;
            return View(topics);
        }

        // Visa inlägg i ett ämne
        public async Task<IActionResult> Topic(int id)
        {
            var topic = await _topicService.GetTopicWithPostsAsync(id);
            if (topic == null)
                return NotFound();

            var posts = await _postService.GetTopLevelPostsByTopicAsync(id);
            ViewBag.Topic = topic;
            ViewBag.UserId = SessionHelper.GetUserId(HttpContext.Session);
            return View(posts);
        }

        // Skapa nytt inlägg
        [HttpPost]
        public async Task<IActionResult> CreatePost(int topicId, string content, int? parentPostId)
        {
            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["ErrorMessage"] = "Innehåll krävs";
                return RedirectToAction("Topic", new { id = topicId });
            }

            await _postService.CreatePostAsync(topicId, userId.Value, content, null, parentPostId);

            TempData["SuccessMessage"] = "Inlägg skapat!";
            return RedirectToAction("Topic", new { id = topicId });
        }
    }
}