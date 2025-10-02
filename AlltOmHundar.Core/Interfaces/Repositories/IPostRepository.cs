using AlltOmHundar.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlltOmHundar.Core.Interfaces.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<IEnumerable<Post>> GetPostsByTopicAsync(int topicId);
        Task<IEnumerable<Post>> GetPostsByUserAsync(int userId);
        Task<Post?> GetPostWithRepliesAsync(int postId);
        Task<IEnumerable<Post>> GetTopLevelPostsByTopicAsync(int topicId);
        Task<IEnumerable<Post>> GetRepliesByParentIdAsync(int parentPostId);
    }
}