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
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IReactionService _reactionService;

        public PostsController(IPostService postService, IReactionService reactionService)
        {
            _postService = postService;
            _reactionService = reactionService;
        }
        ///Hämta alla inlägg för ett ämne
        [HttpGet("topic/{topicId}")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPostsByTopic(int topicId)
        {
            var posts = await _postService.GetTopLevelPostsByTopicAsync(topicId);

            var postDtos = new List<PostDto>();

            foreach (var post in posts)
            {
                var reactionCounts = await _reactionService.GetReactionCountsAsync(post.Id);

                postDtos.Add(new PostDto
                {
                    Id = post.Id,
                    Content = post.Content,
                    ImageUrl = post.ImageUrl,
                    TopicId = post.TopicId,
                    UserId = post.UserId,
                    Username = post.User.Username,
                    UserProfileImageUrl = post.User.ProfileImageUrl,
                    ParentPostId = post.ParentPostId,
                    CreatedAt = post.CreatedAt,
                    UpdatedAt = post.UpdatedAt,
                    ReplyCount = post.Replies.Count,
                    ReactionCounts = reactionCounts.ToDictionary(
                        kvp => kvp.Key.ToString(),
                        kvp => kvp.Value
                    )
                });
            }

            return Ok(postDtos);
        }
    }
}