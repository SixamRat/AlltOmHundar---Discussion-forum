using AlltOmHundar.Core.Enums;
using System;

namespace AlltOmHundar.Core.Models
{
    public class Reaction
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public ReactionType ReactionType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Post Post { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}