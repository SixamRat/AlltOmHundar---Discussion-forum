using AlltOmHundar.Core.Models;
using AlltOmHundar.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlltOmHundar.Core.Interfaces.Repositories
{
    public interface IReactionRepository : IRepository<Reaction>
    {
        Task<Reaction?> GetReactionByUserAndPostAsync(int userId, int postId);
        Task<IEnumerable<Reaction>> GetReactionsByPostAsync(int postId);
        Task<Dictionary<ReactionType, int>> GetReactionCountsByPostAsync(int postId);
    }
}