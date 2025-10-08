using System.ComponentModel.DataAnnotations;

namespace AlltOmHundar.Web.ViewModels
{
    public class SendMessage
    {
        [Required(ErrorMessage = "Välj mottagare")]
        public int ReceiverId { get; set; }

        [Required(ErrorMessage = "Meddelande krävs")]
        [StringLength(1000, ErrorMessage = "Max 1000 tecken")]
        public string Content { get; set; } = string.Empty;
    }
}