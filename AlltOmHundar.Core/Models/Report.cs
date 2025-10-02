using AlltOmHundar.Core.Enums;
using System;

namespace AlltOmHundar.Core.Models
{
    public class Report
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int ReportedByUserId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public ReportStatus Status { get; set; } = ReportStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReviewedAt { get; set; }
        public int? ReviewedByUserId { get; set; }
        public string? AdminNotes { get; set; }

        // Navigation properties
        public Post Post { get; set; } = null!;
        public User ReportedByUser { get; set; } = null!;
    }
}