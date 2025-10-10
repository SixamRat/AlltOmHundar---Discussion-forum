using Microsoft.EntityFrameworkCore;
using AlltOmHundar.Core.Interfaces.Repositories;
using AlltOmHundar.Core.Interfaces.Services;
using AlltOmHundar.Core.Models;
using AlltOmHundar.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlltOmHundar.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupMessageRepository _groupMessageRepository;
        private readonly ApplicationDbContext _context;

        public GroupService(
            IGroupRepository groupRepository,
            IGroupMessageRepository groupMessageRepository,
            ApplicationDbContext context)
        {
            _groupRepository = groupRepository;
            _groupMessageRepository = groupMessageRepository;
            _context = context;
        }

        public async Task<Group> CreateGroupAsync(string name, string? description, int createdByUserId)
        {
            // KOLLA OM NAMNET REDAN FINNS
            var existingGroup = await _context.Groups
                .FirstOrDefaultAsync(g => g.Name.ToLower() == name.ToLower());

            if (existingGroup != null)
            {
                throw new InvalidOperationException("En grupp med detta namn finns redan. Välj ett annat namn.");
            }

            var group = new Group
            {
                Name = name?.Trim() ?? string.Empty,
                Description = string.IsNullOrWhiteSpace(description) ? null : description!.Trim(),
                CreatedByUserId = createdByUserId,
                CreatedAt = DateTime.UtcNow
            };

            await _groupRepository.AddAsync(group);
            await _groupRepository.SaveAsync();

            // Lägg till skaparen som medlem
            await _groupRepository.AddMemberAsync(group.Id, createdByUserId, isAdmin: true);

            return group;
        }

        // NY METOD: Hämta alla grupper
        public async Task<IEnumerable<Group>> GetAllGroupsAsync()
        {
            return await _context.Groups
                .Include(g => g.Members)
                .OrderByDescending(g => g.CreatedAt)
                .ToListAsync();
        }

        
        public async Task<bool> DeleteGroupAsync(int groupId, int userId)
        {
            var group = await _groupRepository.GetByIdAsync(groupId);

            if (group == null)
                return false;

            
            if (group.CreatedByUserId != userId)
                throw new UnauthorizedAccessException("Endast gruppens ägare kan radera gruppen.");

            
            await _groupRepository.DeleteAsync(group);

            return true;
        }

        public async Task<Group?> GetGroupByIdAsync(int id)
        {
            return await _groupRepository.GetByIdAsync(id);
        }

        public async Task<Group?> GetGroupWithMembersAsync(int id)
        {
            return await _groupRepository.GetGroupWithMembersAsync(id);
        }

        public async Task<IEnumerable<Group>> GetUserGroupsAsync(int userId)
        {
            return await _groupRepository.GetUserGroupsAsync(userId);
        }

        public async Task<bool> AddMemberAsync(int groupId, int userId, int requestedByUserId)
        {
            var group = await _groupRepository.GetByIdAsync(groupId);
            if (group == null) return false;

            if (!await _groupRepository.IsAdminOrOwnerAsync(groupId, requestedByUserId))
                return false;

            if (await _groupRepository.IsUserMemberAsync(groupId, userId))
                return false;

            await _groupRepository.AddMemberAsync(groupId, userId);
            return true;
        }

        public async Task<bool> RemoveMemberAsync(int groupId, int userId, int requestedByUserId)
        {
            if (!await _groupRepository.IsAdminOrOwnerAsync(groupId, requestedByUserId))
                return false;

            await _groupRepository.RemoveMemberAsync(groupId, userId);
            return true;
        }

        public async Task<bool> IsUserMemberAsync(int groupId, int userId)
        {
            return await _groupRepository.IsUserMemberAsync(groupId, userId);
        }

        public async Task<GroupMessage> SendGroupMessageAsync(int groupId, int senderId, string content)
        {
            if (!await _groupRepository.IsUserMemberAsync(groupId, senderId))
                throw new InvalidOperationException("Inte medlem ännu");

            var message = new GroupMessage
            {
                GroupId = groupId,
                SenderId = senderId,
                Content = content,
                SentAt = DateTime.UtcNow
            };

            await _groupMessageRepository.AddAsync(message);
            return message;
        }

        public async Task<IEnumerable<GroupMessage>> GetGroupMessagesAsync(int groupId)
        {
            return await _groupMessageRepository.GetMessagesByGroupAsync(groupId);
        }

        public async Task InviteAsync(int groupId, int invitedUserId, int invitedByUserId)
        {
            if (!await _groupRepository.IsAdminOrOwnerAsync(groupId, invitedByUserId))
                throw new UnauthorizedAccessException("Endast ägare/admin kan bjuda in.");

            await _groupRepository.UpsertInviteAsync(groupId, invitedUserId, invitedByUserId);
        }

        public async Task AcceptInviteAsync(int invitationId, int userId)
        {
            var inv = await _groupRepository.GetInviteAsync(invitationId)
                      ?? throw new KeyNotFoundException("Inbjudan hittades inte.");
            if (inv.InvitedUserId != userId) throw new UnauthorizedAccessException();

            inv.Status = "Accepted";
            await _groupRepository.SaveAsync();

            if (!await _groupRepository.IsUserMemberAsync(inv.GroupId, userId))
                await _groupRepository.AddMemberAsync(inv.GroupId, userId);
        }

        public async Task DeclineInviteAsync(int invitationId, int userId)
        {
            var inv = await _groupRepository.GetInviteAsync(invitationId)
                      ?? throw new KeyNotFoundException("Inbjudan hittades inte.");
            if (inv.InvitedUserId != userId) throw new UnauthorizedAccessException();

            inv.Status = "Declined";
            await _groupRepository.SaveAsync();
        }

        public async Task<IEnumerable<GroupInvitation>> GetUserInvitationsAsync(int userId)
        {
            var list = await _groupRepository.GetInvitesForUserAsync(userId);
            return list;
        }
    }
}