using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlltOmHundar.Core.Interfaces.Repositories;
using AlltOmHundar.Core.Models;
using AlltOmHundar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AlltOmHundar.Infrastructure.Repositories
{
    public class GroupMessageRepository : Repository<GroupMessage>, IGroupMessageRepository
    {
        public GroupMessageRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<GroupMessage>> GetMessagesByGroupAsync(int groupId)
        {
            return await _context.GroupMessages
                .Where(m => m.GroupId == groupId)
                .Include(m => m.Sender)
                .OrderBy(m => m.SentAt) 
                .ToListAsync();
        }
    }
}
