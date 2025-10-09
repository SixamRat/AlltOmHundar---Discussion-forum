using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AlltOmHundar.Core.Interfaces.Services;

namespace AlltOmHundar.Web.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        private readonly IGroupService _groups;

        public GroupController(IGroupService groups)
        {
            _groups = groups;
        }

        private int? CurrentUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(idStr, out var id)) return id;
            return null;
        }

        [HttpGet]
        public async Task<IActionResult> My()
        {
            var me = CurrentUserId();
            if (me is null) return Unauthorized();
            var groups = await _groups.GetUserGroupsAsync(me.Value);
            return View(groups);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name, string? description)
        {
            var me = CurrentUserId();
            if (me is null) return Unauthorized();
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["Error"] = "Ange ett gruppnamn.";
                return RedirectToAction(nameof(My));
            }

            await _groups.CreateGroupAsync(name: name, description: string.IsNullOrWhiteSpace(description) ? null : description, createdByUserId: me.Value);
            return RedirectToAction(nameof(My));
        }

        [HttpGet]
        public async Task<IActionResult> Read(int id)
        {
            var me = CurrentUserId();
            if (me is null) return Unauthorized();
            var isMember = await _groups.IsUserMemberAsync(id, me.Value);
            if (!isMember) return Forbid();
            var messages = await _groups.GetGroupMessagesAsync(id);
            ViewBag.GroupId = id;
            return View(messages);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(int groupId, string content)
        {
            var me = CurrentUserId();
            if (me is null) return Unauthorized();
            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["Error"] = "Meddelandet kan inte vara tomt.";
                return RedirectToAction(nameof(Read), new { id = groupId });
            }

            await _groups.SendGroupMessageAsync(groupId, me.Value, content);
            return RedirectToAction(nameof(Read), new { id = groupId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMember(int groupId, int userId)
        {
            var me = CurrentUserId();
            if (me is null) return Unauthorized();
            var ok = await _groups.AddMemberAsync(groupId, userId, me.Value);
            if (!ok) TempData["Error"] = "Du saknar behörighet eller användaren är redan medlem.";
            return RedirectToAction(nameof(Read), new { id = groupId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMember(int groupId, int userId)
        {
            var me = CurrentUserId();
            if (me is null) return Unauthorized();
            var ok = await _groups.RemoveMemberAsync(groupId, userId, me.Value);
            if (!ok) TempData["Error"] = "Du saknar behörighet eller medlemmen finns inte.";
            return RedirectToAction(nameof(Read), new { id = groupId });
        }
    }
}
