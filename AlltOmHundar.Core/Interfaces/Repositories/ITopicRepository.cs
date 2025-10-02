using AlltOmHundar.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlltOmHundar.Core.Interfaces.Repositories
{
    public interface ITopicRepository : IRepository<Topic>
    {
        Task<IEnumerable<Topic>> GetTopicsByCategoryAsync(int categoryId);
        Task<Topic?> GetTopicWithPostsAsync(int topicId);
    }
}