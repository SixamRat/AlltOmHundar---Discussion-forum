using System.ComponentModel.DataAnnotations;

namespace AlltOmHundar.Web.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Användarnamn krävs")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Användarnamn måste vara mellan 3 och 50 tecken")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-post krävs")]
        [EmailAddress(ErrorMessage = "Ogiltig e-postadress")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lösenord krävs")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Lösenord måste vara minst 6 tecken")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bekräfta lösenord")]
        [Compare("Password", ErrorMessage = "Lösenorden matchar inte")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}