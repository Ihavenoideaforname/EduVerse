using System.ComponentModel.DataAnnotations;

namespace EduVerse.Models
{
    public class SchoolAccountViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public string SchoolName { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public Guid SchoolId { get; set; }
        public Guid SchoolAccountId { get; set; }
    }
}
