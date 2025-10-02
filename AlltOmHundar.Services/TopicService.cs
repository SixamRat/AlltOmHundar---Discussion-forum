using AlltOmHundar.Core.Interfaces.Repositories;
using AlltOmHundar.Core.Interfaces.Services;
using AlltOmHundar.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlltOmHundar.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;

        public TopicService(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public async Task<Topic?> GetTopicByIdAsync(int id)
        {
            return await _topicRepository.GetByIdAsync(id);
        }

        public async Task<Topic?> GetTopicWithPostsAsync(int id)
        {
            return await _topicRepository.GetTopicWithPostsAsync(id);
        }

        public async Task<IEnumerable<Topic>> GetTopicsByCategoryAsync(int categoryId)
        {
            return await _topicRepository.GetTopicsByCategoryAsync(categoryId);
        }

        public async Task<Topic> CreateTopicAsync(int categoryId, string title, string? description)
        {
            var topic = new Topic
            {
                CategoryId = categoryId,
                Title = title,
                Description = description,
                CreatedAt = DateTime.UtcNow
            };

            await _topicRepository.AddAsync(topic);
            return topic;
        }

        public async Task<bool> UpdateTopicAsync(int id, string title, string? description)
        {
            var topic = await _topicRepository.GetByIdAsync(id);
            if (topic == null)
                return false;

            topic.Title = title;
            topic.Description = description;

            await _topicRepository.UpdateAsync(topic);
            return true;
        }

        public async Task<bool> DeleteTopicAsync(int id)
        {
            var topic = await _topicRepository.GetByIdAsync(id);
            if (topic == null)
                return false;

            await _topicRepository.DeleteAsync(topic);
            return true;
        }
    }
}