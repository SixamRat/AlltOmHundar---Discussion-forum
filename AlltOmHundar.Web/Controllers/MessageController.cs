using AlltOmHundar.Core.Interfaces.Services;
using AlltOmHundar.Web.Helpers;
using AlltOmHundar.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AlltOmHundar.Web.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;

        public MessageController(IMessageService messageService, IUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        // Inbox - Mottagna meddelanden
        [HttpGet]
        public async Task<IActionResult> Inbox()
        {
            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            var messages = await _messageService.GetReceivedMessagesAsync(userId.Value);
            return View(messages);
        }

        // Skickade meddelanden
        [HttpGet]
        public async Task<IActionResult> Sent()
        {
            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            var messages = await _messageService.GetSentMessagesAsync(userId.Value);
            return View(messages);
        }

        // Visa ett specifikt meddelande
        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            var message = await _messageService.GetMessageByIdAsync(id);
            if (message == null)
                return NotFound();

            // Kolla att användaren är avsändare eller mottagare
            if (message.SenderId != userId.Value && message.ReceiverId != userId.Value)
                return Forbid();

            // Markera som läst om mottagaren öppnar
            if (message.ReceiverId == userId.Value && !message.IsRead)
            {
                await _messageService.MarkAsReadAsync(id);
                message.IsRead = true;
            }

            return View(message);
        }

        // Visa formulär för nytt meddelande
        [HttpGet]
        public async Task<IActionResult> Send(int? receiverId)
        {
            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            var model = new SendMessage();
            if (receiverId.HasValue)
                model.ReceiverId = receiverId.Value;

            // Hämta alla användare för dropdown (utom nuvarande användare)
            var allUsers = await _userService.GetAllUsersAsync();
            ViewBag.Users = allUsers.Where(u => u.Id != userId.Value).ToList();

            return View(model);
        }

        // Skicka meddelande
        [HttpPost]
        public async Task<IActionResult> Send(SendMessage model)
        {
            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                var allUsers = await _userService.GetAllUsersAsync();
                ViewBag.Users = allUsers.Where(u => u.Id != userId.Value).ToList();
                return View(model);
            }

            await _messageService.SendMessageAsync(userId.Value, model.ReceiverId, model.Content);

            TempData["SuccessMessage"] = "Meddelande skickat!";
            return RedirectToAction("Sent");
        }
    }
}