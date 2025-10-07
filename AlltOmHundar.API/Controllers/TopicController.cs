using AlltOmHundar.Core.DTOs;
using AlltOmHundar.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlltOmHundar.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicService _topicService;

        public TopicsController(ITopicService topicService)
        {
            _topicService = topicService;
        }

       
        /// Hämta alla ämnen i en kategori
        
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<TopicDto>>> GetTopicsByCategory(int categoryId)
        {
            var topics = await _topicService.GetTopicsByCategoryAsync(categoryId);

            var topicDtos = topics.Select(t => new TopicDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                CategoryId = t.CategoryId,
                CategoryName = t.Category?.Name ?? "",
                CreatedAt = t.CreatedAt,
                PostCount = t.Posts?.Count ?? 0
            });

            return Ok(topicDtos);
        }

       
        /// Hämta ett specifikt ämne
      
        [HttpGet("{id}")]
        public async Task<ActionResult<TopicDto>> GetTopic(int id)
        {
            var topic = await _topicService.GetTopicByIdAsync(id);

            if (topic == null)
                return NotFound();

            var topicDto = new TopicDto
            {
                Id = topic.Id,
                Title = topic.Title,
                Description = topic.Description,
                CategoryId = topic.CategoryId,
                CategoryName = topic.Category?.Name ?? "",
                CreatedAt = topic.CreatedAt,
                PostCount = topic.Posts?.Count ?? 0
            };

            return Ok(topicDto);
        }
    }
}