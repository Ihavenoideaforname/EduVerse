using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduVerse.Models
{
    public class School
    {
        [Key]
        [Required]
        public Guid Id { get; set; } = new Guid();

        [ForeignKey(nameof(SignUpRequest))]
        public Guid SignUpRequestId { get; set; }
        public virtual SignUpRequest? SignUpRequest { get; set; }

        [Required]
        [MaxLength(200)]
        [DataType(DataType.Text)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        [DataType(DataType.Text)]
        public string NormalizedName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [DataType(DataType.Text)]
        public string NameShortcut { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [DataType(DataType.Text)]
        public string NormalizedNameShortcut { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public string NormalizedEmail { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [DataType(DataType.Text)]
        public string Street { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [DataType(DataType.Text)]
        public string BuildingNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [DataType(DataType.Text)]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [DataType(DataType.Text)]
        public string State { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [DataType(DataType.Text)]
        public string Country { get; set; } = string.Empty;

        public virtual ICollection<SchoolRole> SchoolRoles { get; set; } = new List<SchoolRole>();
        public virtual ICollection<SchoolAccount> SchoolAccounts { get; set; } = new List<SchoolAccount>();
    }
}
