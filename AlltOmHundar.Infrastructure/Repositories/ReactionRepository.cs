using AlltOmHundar.Core.Interfaces.Repositories;
using AlltOmHundar.Core.Models;
using AlltOmHundar.Core.Enums;
using AlltOmHundar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlltOmHundar.Infrastructure.Repositories
{
    public class ReactionRepository : Repository<Reaction>, IReactionRepository
    {
        public ReactionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Reaction?> GetReactionByUserAndPostAsync(int userId, int postId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(r => r.UserId == userId && r.PostId == postId);
        }

        public async Task<IEnumerable<Reaction>> GetReactionsByPostAsync(int postId)
        {
            return await _dbSet
                .Include(r => r.User)
                .Where(r => r.PostId == postId)
                .ToListAsync();
        }

        public async Task<Dictionary<ReactionType, int>> GetReactionCountsByPostAsync(int postId)
        {
            return await _dbSet
                .Where(r => r.PostId == postId)
                .GroupBy(r => r.ReactionType)
                .Select(g => new { ReactionType = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.ReactionType, x => x.Count);
        }
    }
}