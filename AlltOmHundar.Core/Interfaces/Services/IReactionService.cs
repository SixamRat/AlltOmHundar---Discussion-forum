using AlltOmHundar.Core.Models;
using AlltOmHundar.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlltOmHundar.Core.Interfaces.Services
{
    public interface IReactionService
    {
        Task<bool> AddOrUpdateReactionAsync(int postId, int userId, ReactionType reactionType);
        Task<bool> RemoveReactionAsync(int postId, int userId);
        Task<Dictionary<ReactionType, int>> GetReactionCountsAsync(int postId);
        Task<Reaction?> GetUserReactionAsync(int postId, int userId);
    }
}