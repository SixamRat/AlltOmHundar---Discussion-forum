using System;
using System.Collections.Generic;

namespace AlltOmHundar.Core.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Category Category { get; set; } = null!;
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}