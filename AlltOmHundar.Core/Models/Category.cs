using System.Collections.Generic;

namespace AlltOmHundar.Core.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }

        // Navigation properties
        public ICollection<Topic> Topics { get; set; } = new List<Topic>();
    }
}