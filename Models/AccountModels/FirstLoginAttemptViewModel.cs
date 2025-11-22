using System.ComponentModel.DataAnnotations;

namespace EduVerse.Models
{
    public class FirstLoginAttemptViewModel
    {
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Password field is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters and contain an uppercase letter, lowercase letter, number, and special character")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password field is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
