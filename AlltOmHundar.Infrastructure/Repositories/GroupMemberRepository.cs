using AlltOmHundar.Core.Interfaces.Repositories;
using AlltOmHundar.Core.Models;
using AlltOmHundar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AlltOmHundar.Infrastructure.Repositories
{
    public class GroupMemberRepository : Repository<GroupMember>, IGroupMemberRepository
    {
        public GroupMemberRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<GroupMember?> GetMembershipAsync(int groupId, int userId)
        {
            return await _dbSet.FirstOrDefaultAsync(gm => gm.GroupId == groupId && gm.UserId == userId);
        }
    }
}