using AlltOmHundar.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlltOmHundar.Core.Interfaces.Services
{
    public interface IGroupService
    {
        Task<Group> CreateGroupAsync(string name, string? description, int createdByUserId);
        Task<Group?> GetGroupByIdAsync(int id);
        Task<Group?> GetGroupWithMembersAsync(int id);
        Task<IEnumerable<Group>> GetUserGroupsAsync(int userId);
        Task<bool> AddMemberAsync(int groupId, int userId, int requestedByUserId);
        Task<bool> RemoveMemberAsync(int groupId, int userId, int requestedByUserId);
        Task<bool> IsUserMemberAsync(int groupId, int userId);
        Task<GroupMessage> SendGroupMessageAsync(int groupId, int senderId, string content);
        Task<IEnumerable<GroupMessage>> GetGroupMessagesAsync(int groupId);
    }
}