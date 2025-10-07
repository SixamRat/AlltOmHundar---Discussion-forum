using AlltOmHundar.Core.Models;
using System.Threading.Tasks;

namespace AlltOmHundar.Core.Interfaces.Repositories
{
    public interface IGroupMemberRepository : IRepository<GroupMember>
    {
        Task<GroupMember?> GetMembershipAsync(int groupId, int userId);
    }
}