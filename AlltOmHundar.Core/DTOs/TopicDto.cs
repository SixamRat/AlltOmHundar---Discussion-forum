using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlltOmHundar.Core.DTOs
{
    public class TopicDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description {  get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int PostCount { get; set; }
    }
}
