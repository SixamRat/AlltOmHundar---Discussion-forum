using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlltOmHundar.Core.Interfaces.Repositories;
using AlltOmHundar.Core.Models;
using AlltOmHundar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AlltOmHundar.Infrastructure.Repositories
{
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        public GroupRepository(ApplicationDbContext context) : base(context) { }

        

        // Lista grupper där användaren är medlem
        public async Task<IEnumerable<Group>> GetUserGroupsAsync(int userId)
        {
            return await _dbSet
                .Include(g => g.Members)
                .Where(g => g.Members.Any(m => m.UserId == userId))
                .ToListAsync();
        }

        // Hämta grupp med medlemmar + skapare
        public async Task<Group?> GetGroupWithMembersAsync(int groupId)
        {
            return await _dbSet
                .Include(g => g.Members).ThenInclude(m => m.User)
                .Include(g => g.CreatedByUser)
                .FirstOrDefaultAsync(g => g.Id == groupId);
        }
        public async Task<bool> IsUserMemberAsync(int groupId, int userId)
        {
            return await _context.GroupMembers
                .AnyAsync(gm => gm.GroupId == groupId && gm.UserId == userId);
        }
        public async Task<bool> IsAdminOrOwnerAsync(int groupId, int userId)
        {
            var grp = await _dbSet.AsNoTracking().FirstOrDefaultAsync(g => g.Id == groupId);
            if (grp is null) return false;
            if (grp.CreatedByUserId == userId) return true;

            return await _context.GroupMembers
                .AnyAsync(m => m.GroupId == groupId && m.UserId == userId && m.IsAdmin);
        }

        // Lägg till medlem
        public async Task AddMemberAsync(int groupId, int userId, bool isAdmin = false)
        {
            var exists = await _context.GroupMembers
                .AnyAsync(m => m.GroupId == groupId && m.UserId == userId);

            if (!exists)
            {
                _context.GroupMembers.Add(new GroupMember
                {
                    GroupId = groupId,
                    UserId = userId,
                    IsAdmin = isAdmin,
                    JoinedAt = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();
            }
        }

        // Ta bort medlem
        public async Task RemoveMemberAsync(int groupId, int targetUserId)
        {
            var member = await _context.GroupMembers
                .FirstOrDefaultAsync(m => m.GroupId == groupId && m.UserId == targetUserId);

            if (member is null) return;

            _context.GroupMembers.Remove(member);
            await _context.SaveChangesAsync();
        }
        public async Task<GroupInvitation> UpsertInviteAsync(int groupId, int invitedUserId, int invitedByUserId)
        {
            var inv = await _context.GroupInvitations
                .FirstOrDefaultAsync(i => i.GroupId == groupId && i.InvitedUserId == invitedUserId);

            if (inv is null)
            {
                inv = new GroupInvitation
                {
                    GroupId = groupId,
                    InvitedUserId = invitedUserId,
                    InvitedByUserId = invitedByUserId,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow
                };
                _context.GroupInvitations.Add(inv);
            }
            else
            {
                inv.Status = "Pending";
            }

            await _context.SaveChangesAsync();
            return inv;
        }

        public async Task<GroupInvitation?> GetInviteAsync(int invitationId)
        {
            return await _context.GroupInvitations
                .Include(i => i.Group)
                .Include(i => i.InvitedUser)
                .Include(i => i.InvitedByUser)
                .FirstOrDefaultAsync(i => i.Id == invitationId);
        }

        public async Task<IReadOnlyList<GroupInvitation>> GetInvitesForUserAsync(int userId)
        {
            var list = await _context.GroupInvitations
                .Where(i => i.InvitedUserId == userId && i.Status == "Pending")
                .Include(i => i.Group)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            return list;
        }
        public async Task<GroupMessage> AddMessageAsync(GroupMessage msg)
        {
            _context.GroupMessages.Add(msg);
            await _context.SaveChangesAsync();
            return msg;
        }

        public async Task<IReadOnlyList<GroupMessage>> GetMessagesAsync(int groupId)
        {
            var list = await _context.GroupMessages
                .Where(m => m.GroupId == groupId)
                .Include(m => m.Sender)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();

            return list;
        }
        public Task SaveAsync() => _context.SaveChangesAsync();
    }
}
