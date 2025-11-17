using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace EduVerse.Models
{
    public class User : IdentityUser<Guid>
    {
        public Guid? SchoolAccountId { get; set; }
        public virtual SchoolAccount? SchoolAccount { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(50)]
        public string Surname { get; set; } = string.Empty;

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
