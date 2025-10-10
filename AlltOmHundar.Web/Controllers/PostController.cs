using System;
using System.IO;
using System.Threading.Tasks;
using AlltOmHundar.Core.Interfaces.Services;
using AlltOmHundar.Web.Helpers;
using AlltOmHundar.Web.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AlltOmHundar.Web.Controllers
{
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
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            ViewBag.TopicId = topicId;
            ViewBag.ParentPostId = parentPostId;

            return View(new CreatePost { ParentPostId = parentPostId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(int topicId, CreatePost model)
        {
            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                ViewBag.TopicId = topicId;
                ViewBag.ParentPostId = model.ParentPostId;
                return View(model);
            }

            string? imageUrl = null;

            // Hantera bild om den finns
            if (model.Image != null && model.Image.Length > 0)
            {
                var ext = Path.GetExtension(model.Image.FileName).ToLowerInvariant();
                var allowedExt = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                if (!Array.Exists(allowedExt, e => e == ext))
                {
                    ModelState.AddModelError("Image", "Endast jpg, png eller gif tillåtet");
                    ViewBag.TopicId = topicId;
                    ViewBag.ParentPostId = model.ParentPostId;
                    return View(model);
                }

                if (model.Image.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("Image", "Max 5MB");
                    ViewBag.TopicId = topicId;
                    ViewBag.ParentPostId = model.ParentPostId;
                    return View(model);
                }

                // Spara bild
                var fileName = $"post_{Guid.NewGuid()}{ext}";
                var uploadsFolder = Path.Combine("wwwroot", "images", "posts");

                // Se till att mappen finns
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                imageUrl = $"/images/posts/{fileName}";
            }

            // Skapa inlägg
            await _postService.CreatePostAsync(
                topicId,
                userId.Value,
                model.Content,
                model.ParentPostId,
                imageUrl
            );

            TempData["SuccessMessage"] = model.ParentPostId.HasValue ? "Svar skapat!" : "Inlägg skapat!";
            return RedirectToAction("Topic", "Forum", new { id = topicId });
        }
    }
}