using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AlltOmHundar.Web.ViewModels
{
    public class ProfileImage
    {
        [Required(ErrorMessage = "Välj din bild")]
        public IFormFile Image { get; set; } = null!;
    }
}
