using System;
using System.Collections.Generic;
using static System.Collections.Specialized.BitVector32;

namespace AlltOmHundar.Core.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int TopicId { get; set; }
        public int UserId { get; set; }
        public int? ParentPostId { get; set; } // För trädstruktur (svar på inlägg)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;

        // Navigation properties
        public Topic Topic { get; set; } = null!;
        public User User { get; set; } = null!;
        public Post? ParentPost { get; set; }
        public ICollection<Post> Replies { get; set; } = new List<Post>();
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}