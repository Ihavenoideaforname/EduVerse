using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace EduVerse.Models
{
    public class SchoolAccount
    {
        [Key]
        public Guid Id { get; set; }

        //Shared

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public virtual User? User { get; set; }

        [ForeignKey(nameof(School))]
        public Guid SchoolId { get; set; }
        public virtual School? School { get; set; }

        [ForeignKey(nameof(SchoolRole))]
        public Guid SchoolRoleId { get; set; }
        public virtual SchoolRole? SchoolRole { get; set; }

        //Student Only

        [ForeignKey(nameof(Group))]
        public Guid? GroupId { get; set; }
        public virtual Group? Group { get; set;}

        public virtual ICollection<StudentParents> Parents { get; set; } = new List<StudentParents>();

        //Parents Only

        public virtual ICollection<StudentParents> Students { get; set; } = new List<StudentParents>();

        //Staff Only

        [InverseProperty(nameof(EduVerse.Models.Group.Teacher))]
        public virtual Group? SupervisedGroup { get; set; }

        //Add courses here
    }
}
