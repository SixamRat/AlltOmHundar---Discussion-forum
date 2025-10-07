using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AlltOmHundar.Core.DTOs
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int TopicId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? UserProfileImageUrl { get; set; }
        public int? ParentPostId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } 
        public int ReplyCount { get; set; }
        public Dictionary<string, int> ReactionCounts { get; set; } = new Dictionary<string, int>();


    }
}
