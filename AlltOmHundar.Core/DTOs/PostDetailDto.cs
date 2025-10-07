using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlltOmHundar.Core.DTOs
{
    public class PostDetailDto
    {        
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int TopicId { get; set; }
        public string TopicTitle { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? UserProfileImageUrl { get; set; }
        public int? ParentPostId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List <PostDto> Replies  { get; set; } = new List<PostDto> ();
        public Dictionary<string, int> ReactionCounts { get; set; } = new Dictionary<string, int>();
    }
}
