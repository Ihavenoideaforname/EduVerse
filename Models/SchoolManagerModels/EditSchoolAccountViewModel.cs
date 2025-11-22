using System.ComponentModel.DataAnnotations;

namespace EduVerse.Models
{
    public class EditSchoolAccountViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(100, ErrorMessage = "Name can't exceed 100 characters.")]
        [RegularExpression(@"^[\p{L}\s\-]{1,100}$", ErrorMessage = "Name can only contain letters, spaces and hyphens.")]
        [DataType(DataType.Text)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Surname is required.")]
        [MaxLength(100, ErrorMessage = "Surname can't exceed 100 characters.")]
        [RegularExpression(@"^[\p{L}\s\-]{1,100}$", ErrorMessage = "Surname can only contain letters, spaces and hyphens.")]
        [DataType(DataType.Text)]
        public string Surname { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Guid SchoolAccountId { get; set; }
        public Guid SchoolId { get; set; }
        public Guid SchoolRoleId { get; set; }
    }
}
