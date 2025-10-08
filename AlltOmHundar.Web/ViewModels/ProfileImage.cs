using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AlltOmHundar.Web.ViewModels
{
    public class ProfileImage
    {
        [Required(ErrorMessage = "Välj din bild")]
        [Display(Name = "Profilbild")]
        public IFormFile Image { get; set; } = null!;
    }
}
