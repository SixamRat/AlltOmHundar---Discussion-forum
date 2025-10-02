using Microsoft.EntityFrameworkCore;
using AlltOmHundar.Core.Interfaces.Repositories;
using AlltOmHundar.Core.Interfaces.Services;
using AlltOmHundar.Core.Models;
using AlltOmHundar.Infrastructure.Data;
using System;
using System.Collections.Generic;
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
            var group = new Group
            {
                Name = name,
                Description = description,
                CreatedByUserId = createdByUserId,
                CreatedAt = DateTime.UtcNow
            };

            await _groupRepository.AddAsync(group);

            // Lägg till skaparen som medlem
            var member = new GroupMember
            {
                GroupId = group.Id,
                UserId = createdByUserId,
                JoinedAt = DateTime.UtcNow
            };
            _context.GroupMembers.Add(member);
            await _context.SaveChangesAsync();

            return group;
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
            if (group == null)
                return false;

            // Kolla att den som lägger till är skaparen
            if (group.CreatedByUserId != requestedByUserId)
                return false;

            // Kolla att användaren inte redan är medlem
            if (await _groupRepository.IsUserMemberAsync(groupId, userId))
                return false;

            var member = new GroupMember
            {
                GroupId = groupId,
                UserId = userId,
                JoinedAt = DateTime.UtcNow
            };

            _context.GroupMembers.Add(member);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveMemberAsync(int groupId, int userId, int requestedByUserId)
        {
            var group = await _groupRepository.GetByIdAsync(groupId);
            if (group == null)
                return false;

            // Kolla att den som tar bort är skaparen
            if (group.CreatedByUserId != requestedByUserId)
                return false;

            var member = await _context.GroupMembers
                .FirstOrDefaultAsync(gm => gm.GroupId == groupId && gm.UserId == userId);

            if (member == null)
                return false;

            _context.GroupMembers.Remove(member);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsUserMemberAsync(int groupId, int userId)
        {
            return await _groupRepository.IsUserMemberAsync(groupId, userId);
        }

        public async Task<GroupMessage> SendGroupMessageAsync(int groupId, int senderId, string content)
        {
            // Kolla att användaren är medlem
            if (!await _groupRepository.IsUserMemberAsync(groupId, senderId))
                throw new InvalidOperationException("User is not a member of this group");

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
    }
}