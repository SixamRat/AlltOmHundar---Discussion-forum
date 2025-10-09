using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AlltOmHundar.Web.ViewModels
{
    public class CreatePost
    {
        [Required(ErrorMessage = "Innehåll krävs")]
        public string Content { get; set; } = string.Empty;

        public int? ParentPostId { get; set; }

       
        [Display(Name = "Bild (valfritt)")]
        public IFormFile? Image { get; set; }
    }
}