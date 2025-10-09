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

            var groups = await _groups.GetMyGroupsAsync(me.Value);
            var invites = await _groups.GetMyInvitesAsync(me.Value);
            return View((groups, invites));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name, string? description)
        {
            var me = CurrentUserId();
            if (me is null) return Unauthorized();
            if (string.IsNullOrWhiteSpace(name)) { ModelState.AddModelError("", "Ange gruppnamn."); return RedirectToAction(nameof(My)); }

            await _groups.CreateGroupAsync(me.Value, name.Trim(), description?.Trim());
            return RedirectToAction(nameof(My));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Invite(int groupId, int userId)
        {
            var me = CurrentUserId();
            if (me is null) return Unauthorized();

            await _groups.InviteAsync(groupId, userId, me.Value);
            return RedirectToAction(nameof(My));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(int invitationId)
        {
            var me = CurrentUserId();
            if (me is null) return Unauthorized();

            await _groups.AcceptInviteAsync(invitationId, me.Value);
            return RedirectToAction(nameof(My));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Decline(int invitationId)
        {
            var me = CurrentUserId();
            if (me is null) return Unauthorized();

            await _groups.DeclineInviteAsync(invitationId, me.Value);
            return RedirectToAction(nameof(My));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMember(int groupId, int targetUserId)
        {
            var me = CurrentUserId();
            if (me is null) return Unauthorized();

            await _groups.RemoveMemberAsync(groupId, targetUserId, me.Value);
            return RedirectToAction(nameof(Read), new { id = groupId });
        }

        [HttpGet]
        public async Task<IActionResult> Read(int id) // id = groupId
        {
            var me = CurrentUserId();
            if (me is null) return Unauthorized();

            var msgs = await _groups.GetMessagesAsync(id, me.Value);
            ViewBag.GroupId = id;
            return View(msgs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(int groupId, string content)
        {
            var me = CurrentUserId();
            if (me is null) return Unauthorized();
            await _groups.SendMessageAsync(groupId, me.Value, content);
            return RedirectToAction(nameof(Read), new { id = groupId });
        }
    }
}
