using AlltOmHundar.Core.Interfaces.Repositories;
using AlltOmHundar.Core.Interfaces.Services;
using AlltOmHundar.Core.Models;
using AlltOmHundar.Core.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlltOmHundar.Services
{
    public class ReactionService : IReactionService
    {
        private readonly IReactionRepository _reactionRepository;

        public ReactionService(IReactionRepository reactionRepository)
        {
            _reactionRepository = reactionRepository;
        }

        public async Task<bool> AddOrUpdateReactionAsync(int postId, int userId, ReactionType reactionType)
        {
            var existingReaction = await _reactionRepository.GetReactionByUserAndPostAsync(userId, postId);

            if (existingReaction != null)
            {
                // Uppdatera befintlig reaktion
                existingReaction.ReactionType = reactionType;
                existingReaction.CreatedAt = DateTime.UtcNow;
                await _reactionRepository.UpdateAsync(existingReaction);
            }
            else
            {
                // Skapa ny reaktion
                var reaction = new Reaction
                {
                    PostId = postId,
                    UserId = userId,
                    ReactionType = reactionType,
                    CreatedAt = DateTime.UtcNow
                };
                await _reactionRepository.AddAsync(reaction);
            }

            return true;
        }

        public async Task<bool> RemoveReactionAsync(int postId, int userId)
        {
            var reaction = await _reactionRepository.GetReactionByUserAndPostAsync(userId, postId);
            if (reaction == null)
                return false;

            await _reactionRepository.DeleteAsync(reaction);
            return true;
        }

        public async Task<Dictionary<ReactionType, int>> GetReactionCountsAsync(int postId)
        {
            return await _reactionRepository.GetReactionCountsByPostAsync(postId);
        }

        public async Task<Reaction?> GetUserReactionAsync(int postId, int userId)
        {
            return await _reactionRepository.GetReactionByUserAndPostAsync(userId, postId);
        }
    }
}