using AlltOmHundar.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static System.Collections.Specialized.BitVector32;

namespace AlltOmHundar.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public string? Bio { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }

        // Navigation properties
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
        public ICollection<Report> Reports { get; set; } = new List<Report>();
        public ICollection<PrivateMessage> SentMessages { get; set; } = new List<PrivateMessage>();
        public ICollection<PrivateMessage> ReceivedMessages { get; set; } = new List<PrivateMessage>();
        public ICollection<GroupMember> GroupMemberships { get; set; } = new List<GroupMember>();
        public ICollection<Group> CreatedGroups { get; set; } = new List<Group>();
        public ICollection<GroupInvitation> GroupInvitations { get; set; } = new List<GroupInvitation>();
    }
}