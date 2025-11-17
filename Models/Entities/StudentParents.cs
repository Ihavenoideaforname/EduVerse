using System.ComponentModel.DataAnnotations.Schema;

namespace EduVerse.Models
{
    public class StudentParents
    {
        [ForeignKey(nameof(Student))]
        public Guid StudentId { get; set; }
        public virtual SchoolAccount? Student { get; set; }

        [ForeignKey(nameof(Parent))]
        public Guid ParentId { get; set; }
        public virtual SchoolAccount? Parent { get; set; }
    }
}
