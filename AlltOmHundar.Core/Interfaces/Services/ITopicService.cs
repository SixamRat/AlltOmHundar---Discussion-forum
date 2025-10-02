using AlltOmHundar.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlltOmHundar.Core.Interfaces.Services
{
    public interface ITopicService
    {
        Task<Topic?> GetTopicByIdAsync(int id);
        Task<Topic?> GetTopicWithPostsAsync(int id);
        Task<IEnumerable<Topic>> GetTopicsByCategoryAsync(int categoryId);
        Task<Topic> CreateTopicAsync(int categoryId, string title, string? description);
        Task<bool> UpdateTopicAsync(int id, string title, string? description);
        Task<bool> DeleteTopicAsync(int id);
    }
}