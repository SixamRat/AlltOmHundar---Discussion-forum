using AlltOmHundar.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlltOmHundar.Core.Interfaces.Services
{
    public interface IPostService
    {
        Task<Post?> GetPostByIdAsync(int id);
        Task<Post?> GetPostWithRepliesAsync(int id);
        Task<IEnumerable<Post>> GetPostsByTopicAsync(int topicId);
        Task<IEnumerable<Post>> GetTopLevelPostsByTopicAsync(int topicId);
        Task<IEnumerable<Post>> GetPostsByUserAsync(int userId);
        Task<Post> CreatePostAsync(int topicId, int userId, string content, int? parentPostId = null, string? imageUrl = null);
        Task<bool> UpdatePostAsync(int postId, int userId, string content);
        Task<bool> DeletePostAsync(int postId, int userId);
        string FilterProfanity(string content);
    }
}