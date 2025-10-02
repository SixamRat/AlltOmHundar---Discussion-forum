using AlltOmHundar.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlltOmHundar.Core.Interfaces.Repositories
{
    public interface IGroupRepository : IRepository<Group>
    {
        Task<IEnumerable<Group>> GetUserGroupsAsync(int userId);
        Task<Group?> GetGroupWithMembersAsync(int groupId);
        Task<bool> IsUserMemberAsync(int groupId, int userId);
    }
}