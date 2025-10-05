using System.ComponentModel.DataAnnotations;

namespace AlltOmHundar.Web.ViewModels
{
    public class CreateTopicViewModel
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Titel krävs")]
        [StringLength(200, ErrorMessage = "Max 200 tecken")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Max 1000 tecken")]
        public string? Description { get; set; }
    }
}