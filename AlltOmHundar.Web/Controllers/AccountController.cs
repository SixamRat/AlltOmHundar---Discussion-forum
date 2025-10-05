using AlltOmHundar.Core.Interfaces.Services;
using AlltOmHundar.Web.Helpers;
using AlltOmHundar.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AlltOmHundar.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            // Om användaren redan är inloggad, skicka till startsidan
            if (SessionHelper.IsLoggedIn(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var user = await _userService.RegisterUserAsync(model.Username, model.Email, model.Password);

                // Logga in användaren direkt efter registrering
                SessionHelper.SetUser(HttpContext.Session, user.Id, user.Username, user.Role.ToString());

                TempData["SuccessMessage"] = "Välkommen till AlltOmHundar!";
                return RedirectToAction("Index", "Forum");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Om användaren redan är inloggad, skicka till startsidan
            if (SessionHelper.IsLoggedIn(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userService.AuthenticateAsync(model.Email, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Felaktig e-post eller lösenord");
                return View(model);
            }

            SessionHelper.SetUser(HttpContext.Session, user.Id, user.Username, user.Role.ToString());

            TempData["SuccessMessage"] = $"Välkommen tillbaka, {user.Username}!";
            return RedirectToAction("Index", "Forum");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            SessionHelper.ClearUser(HttpContext.Session);
            TempData["SuccessMessage"] = "Du har loggats ut";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (!userId.HasValue)
                return RedirectToAction("Login");

            var user = await _userService.GetUserByIdAsync(userId.Value);
            if (user == null)
                return RedirectToAction("Login");

            return View(user);
        }
    }
}