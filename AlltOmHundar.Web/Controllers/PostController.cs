using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AlltOmHundar.Core.Interfaces.Services;
using AlltOmHundar.Web.Helpers;
using AlltOmHundar.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AlltOmHundar.Web.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IWebHostEnvironment _env;

        public PostController(IPostService postService, IWebHostEnvironment env)
        {
            _postService = postService;
            _env = env;
        }

        [HttpGet]
        public IActionResult CreatePost(int topicId, int? parentPostId)
        {
            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (!userId.HasValue) return RedirectToAction("Login", "Account");

            ViewBag.TopicId = topicId;
            return View("/Views/Topic/NewPost.cshtml", new CreatePost { ParentPostId = parentPostId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(int topicId, CreatePost model)
        {
            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (!userId.HasValue) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                ViewBag.TopicId = topicId;
                return View("/Views/Topic/NewPost.cshtml", model);
            }

            string? imageUrl = null;

            if (model.Image is { Length: > 0 })
            {
                var allowedExt = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var ext = Path.GetExtension(model.Image.FileName).ToLowerInvariant();
                if (!allowedExt.Contains(ext))
                {
                    ModelState.AddModelError(nameof(model.Image), "Endast JPG/PNG/GIF/WEBP tillåtet.");
                    ViewBag.TopicId = topicId;
                    return View("/Views/Topic/NewPost.cshtml", model);
                }

                const long maxBytes = 5L * 1024 * 1024;
                if (model.Image.Length > maxBytes)
                {
                    ModelState.AddModelError(nameof(model.Image), "Max 5 MB.");
                    ViewBag.TopicId = topicId;
                    return View("/Views/Topic/NewPost.cshtml", model);
                }

                // Lokalt -> wwwroot/images/posts, Azure -> App_Data/uploads
                var root = _env.IsDevelopment()
                    ? Path.Combine(_env.WebRootPath ?? "wwwroot", "images", "posts")
                    : Path.Combine(_env.ContentRootPath, "App_Data", "uploads");

                Directory.CreateDirectory(root);
                var fileName = $"post_{Guid.NewGuid():N}{ext}";
                var fullPath = Path.Combine(root, fileName);

                try
                {
                    await using var stream = System.IO.File.Create(fullPath);
                    await model.Image.CopyToAsync(stream);
                }
                catch
                {
                    ModelState.AddModelError(nameof(model.Image), "Kunde inte spara bilden.");
                    ViewBag.TopicId = topicId;
                    return View("/Views/Topic/NewPost.cshtml", model);
                }

                imageUrl = _env.IsDevelopment()
                    ? $"/images/posts/{fileName}"
                    : $"/uploads/{fileName}";
            }

            await _postService.CreatePostAsync(
                topicId: topicId,
                userId: userId.Value,
                content: model.Content,
                parentPostId: model.ParentPostId,
                imageUrl: imageUrl
            );

            TempData["SuccessMessage"] = "Inlägg skapat!";
            return RedirectToAction("Topic", "Forum", new { id = topicId });
        }
    }
}
