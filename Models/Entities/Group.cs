using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduVerse.Models
{
    public class Group
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Teacher))]
        public Guid TeacherId { get; set; }
        [InverseProperty(nameof(EduVerse.Models.SchoolAccount.SupervisedGroup))]
        public virtual SchoolAccount? Teacher {  get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(20)]
        public string NormalizedName { get; set; } = string.Empty;

        public virtual ICollection<SchoolAccount> Students { get; set;} = new List<SchoolAccount>();
    }
}
