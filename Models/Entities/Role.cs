using Microsoft.AspNetCore.Identity;

namespace EduVerse.Models
{
    public class Role : IdentityRole<Guid>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
