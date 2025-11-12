using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EduVerse.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email field is required")]
        [DataType(DataType.Text)]
        public string UserIdentifier { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DisplayName("Remember Me")]
        public bool RememberMe { get; set; } = false;
    }
}
