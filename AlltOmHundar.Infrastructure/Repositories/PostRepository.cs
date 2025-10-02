using AlltOmHundar.Core.Interfaces.Repositories;
using AlltOmHundar.Core.Models;
using AlltOmHundar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlltOmHundar.Infrastructure.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Post>> GetPostsByTopicAsync(int topicId)
        {
            return await _dbSet
                .Include(p => p.User)
                .Include(p => p.Reactions)
                .Where(p => p.TopicId == topicId && !p.IsDeleted)
                .OrderBy(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByUserAsync(int userId)
        {
            return await _dbSet
                .Include(p => p.Topic)
                .Where(p => p.UserId == userId && !p.IsDeleted)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Post?> GetPostWithRepliesAsync(int postId)
        {
            return await _dbSet
                .Include(p => p.User)
                .Include(p => p.Replies)
                    .ThenInclude(r => r.User)
                .Include(p => p.Reactions)
                .FirstOrDefaultAsync(p => p.Id == postId);
        }

        public async Task<IEnumerable<Post>> GetTopLevelPostsByTopicAsync(int topicId)
        {
            return await _dbSet
                .Include(p => p.User)
                .Include(p => p.Reactions)
                .Include(p => p.Replies)
                .Where(p => p.TopicId == topicId && p.ParentPostId == null && !p.IsDeleted)
                .OrderBy(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetRepliesByParentIdAsync(int parentPostId)
        {
            return await _dbSet
                .Include(p => p.User)
                .Include(p => p.Reactions)
                .Where(p => p.ParentPostId == parentPostId && !p.IsDeleted)
                .OrderBy(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}