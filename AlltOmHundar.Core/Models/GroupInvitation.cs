using System;

namespace AlltOmHundar.Core.Models
{
    public class GroupInvitation
    {
        public int Id { get; set; }

        
        public int GroupId { get; set; }
        public Group Group { get; set; } = null!;

        
        public int InvitedUserId { get; set; }
        public User InvitedUser { get; set; } = null!;

        
        public int InvitedByUserId { get; set; }
        public User InvitedByUser { get; set; } = null!;

        
        public string Status { get; set; } = "Inväntar godkännande";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
