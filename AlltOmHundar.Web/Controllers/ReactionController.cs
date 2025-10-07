using AlltOmHundar.Core.Enums;
using AlltOmHundar.Core.Interfaces.Services;
using AlltOmHundar.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlltOmHundar.Web.Controllers
{
    public class ReactionController : Controller
    {
        private readonly IReactionService _reactionService;

        public ReactionController(IReactionService reactionService)
        {
            _reactionService = reactionService;
        }
        [HttpPost]
        public async Task<IActionResult> AddReaction(int postId, ReactionType reactionType, int topicId)
        {
            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (!userId.HasValue)
            {
                TempData["ErrorMessage"] = "Du måste vara inloggad för att reagera på detta inlägg";
                return RedirectToAction("Login", "Account");
            }
            await _reactionService.AddOrUpdateReactionAsync(postId, userId.Value, reactionType);
            return RedirectToAction("Topic", "Forum", new { id= topicId });
        }
        [HttpPost]
        public async Task<IActionResult> RemoveReaction(int postId, int topicId)
        {
            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }
            await _reactionService.RemoveReactionAsync(postId, userId.Value);
            return RedirectToAction("Topic", "Forum", new { id = topicId });
        }
    }
}
