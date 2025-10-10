using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AlltOmHundar.Core.Interfaces.Services;
using AlltOmHundar.Web.Helpers;

namespace AlltOmHundar.Web.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGroupService _groups;
        private readonly IUserService _users;

        public GroupController(IGroupService groups, IUserService users)
        {
            _groups = groups;
            _users = users;
        }

        private int? GetCurrentUserId()
        {
            return SessionHelper.GetUserId(HttpContext.Session);
        }

        [HttpGet]
        public async Task<IActionResult> My()
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            var groups = await _groups.GetUserGroupsAsync(userId.Value);

            try
            {
                ViewBag.Invites = await _groups.GetUserInvitationsAsync(userId.Value);
            }
            catch
            {
                ViewBag.Invites = null;
            }

            return View("MyGroup", groups);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name, string? description)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["Error"] = "Ange ett gruppnamn.";
                return RedirectToAction(nameof(My));
            }

            try
            {
                await _groups.CreateGroupAsync(name, description, userId.Value);
                TempData["SuccessMessage"] = "Grupp skapad!";
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = $"Kunde inte skapa grupp: {ex.Message}";
            }

            return RedirectToAction(nameof(My));
        }

        [HttpGet]
        public async Task<IActionResult> Read(int id)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            var isMember = await _groups.IsUserMemberAsync(id, userId.Value);
            if (!isMember)
            {
                TempData["Error"] = "Du är inte medlem i denna grupp.";
                return RedirectToAction(nameof(My));
            }

            var messages = await _groups.GetGroupMessagesAsync(id);
            var group = await _groups.GetGroupWithMembersAsync(id);

            ViewBag.GroupId = id;
            ViewBag.GroupName = group?.Name;

            return View("GroupChat", messages);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(int groupId, string content)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["Error"] = "Meddelandet kan inte vara tomt.";
                return RedirectToAction(nameof(Read), new { id = groupId });
            }

            try
            {
                await _groups.SendGroupMessageAsync(groupId, userId.Value, content);
                TempData["SuccessMessage"] = "Meddelande skickat!";
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Read), new { id = groupId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Invite(int groupId, int userId)
        {
            var currentUserId = GetCurrentUserId();
            if (!currentUserId.HasValue)
                return RedirectToAction("Login", "Account");

            try
            {
                await _groups.InviteAsync(groupId, userId, currentUserId.Value);
                TempData["SuccessMessage"] = "Inbjudan skickad!";
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Members), new { id = groupId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(int invitationId)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            try
            {
                await _groups.AcceptInviteAsync(invitationId, userId.Value);
                TempData["SuccessMessage"] = "Inbjudan accepterad!";
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(My));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Decline(int invitationId)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            try
            {
                await _groups.DeclineInviteAsync(invitationId, userId.Value);
                TempData["SuccessMessage"] = "Inbjudan avböjd.";
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(My));
        }

        [HttpGet]
        public async Task<IActionResult> Members(int id)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            var group = await _groups.GetGroupWithMembersAsync(id);
            if (group == null)
                return NotFound();

            var allUsers = await _users.GetAllUsersAsync();
            ViewBag.AllUsers = allUsers.Where(u => !group.Members.Any(m => m.UserId == u.Id)).ToList();
            ViewBag.GroupId = id;

            return View(group);
        }
        [HttpGet]
        public async Task<IActionResult> Browse()
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            var allGroups = await _groups.GetAllGroupsAsync();
            var userGroups = await _groups.GetUserGroupsAsync(userId.Value);
            var userGroupIds = userGroups.Select(g => g.Id).ToHashSet();

            ViewBag.UserGroupIds = userGroupIds;

            return View(allGroups);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(int groupId)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            try
            {
                // Lägg till användaren direkt som medlem
                await _groups.AddMemberAsync(groupId, userId.Value, userId.Value);
                TempData["SuccessMessage"] = "Du har gått med i gruppen!";
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Browse));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            try
            {
                var success = await _groups.DeleteGroupAsync(id, userId.Value);
                if (success)
                    TempData["SuccessMessage"] = "Grupp raderad!";
                else
                    TempData["Error"] = "Gruppen kunde inte raderas.";
            }
            catch (UnauthorizedAccessException)
            {
                TempData["Error"] = "Endast gruppens ägare kan radera gruppen.";
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(My));
        }
    }
}