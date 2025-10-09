using System;
using System.Collections.Generic;

namespace AlltOmHundar.Core.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public User CreatedByUser { get; set; } = null!;
        public ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();
        public ICollection<GroupMessage> Messages { get; set; } = new List<GroupMessage>();
        public ICollection<GroupInvitation> Invitations { get; set; } = new List<GroupInvitation>();
    }
}