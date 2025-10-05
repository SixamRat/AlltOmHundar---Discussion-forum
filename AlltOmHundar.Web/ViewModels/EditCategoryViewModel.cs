using System.ComponentModel.DataAnnotations;

namespace AlltOmHundar.Web.ViewModels
{
    public class EditCategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Namn krävs")]
        [StringLength(100, ErrorMessage = "Max 100 tecken")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Max 500 tecken")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Visningsordning krävs")]
        [Range(1, 100, ErrorMessage = "Måste vara mellan 1 och 100")]
        public int DisplayOrder { get; set; }
    }
}