using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduVerse.Models
{
    public class SchoolRole
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey(nameof(School))]
        public Guid SchoolId { get; set; }
        public virtual School? School { get; set; }

        [Required]
        [MaxLength(50)]
        [DataType(DataType.Text)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [DataType(DataType.Text)]
        public string NormalizedName {  get; set; } = string.Empty;

        [Required]
        public int Hierarchy { get; set; }

        [Required]
        public bool IsParent { get; set; }

        [Required]
        public bool IsStudent { get; set; }

        [Required]
        public bool IsStaff { get; set; }

        [Required]
        public bool CanManageAccounts { get; set; }
        
        [Required]
        public bool CanManageRoles { get; set; }

        [Required]
        public bool CanManageGroups { get; set; }

        [Required]
        public bool CanManageCourses { get; set; }

        [Required]
        public bool CanManageStudents { get; set; }

        public virtual ICollection<SchoolAccount> SchoolAccounts { get; set; } = new List<SchoolAccount>();
    }
}
