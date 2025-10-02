using AlltOmHundar.Core.Interfaces.Repositories;
using AlltOmHundar.Core.Models;
using AlltOmHundar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlltOmHundar.Infrastructure.Repositories
{
    public class TopicRepository : Repository<Topic>, ITopicRepository
    {
        public TopicRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Topic>> GetTopicsByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Where(t => t.CategoryId == categoryId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<Topic?> GetTopicWithPostsAsync(int topicId)
        {
            return await _dbSet
                .Include(t => t.Posts)
                    .ThenInclude(p => p.User)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == topicId);
        }
    }
}