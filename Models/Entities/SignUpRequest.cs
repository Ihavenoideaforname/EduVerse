using System.ComponentModel.DataAnnotations;
using EduVerse.Enums;

namespace EduVerse.Models
{
    public class SignUpRequest
    {
        [Key]
        [Required]
        public Guid Id { get; set; } = new Guid();

        [Required]
        [MaxLength(200)]
        [DataType(DataType.Text)]
        public string SchoolName { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        [DataType(DataType.Text)]
        public string NormalizedSchoolName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [DataType(DataType.Text)]
        public string SchoolNameShortcut {  get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [DataType(DataType.Text)]
        public string NormalizedSchoolNameShortcut { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public string SchoolEmail { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public string NormalizedSchoolEmail { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [DataType(DataType.PhoneNumber)]
        public string SchoolPhoneNumber { get; set; } = string.Empty;

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

        [Required]
        [MaxLength(100)]
        [DataType(DataType.Text)]
        public string PrincipalName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [DataType(DataType.Text)]
        public string PrincipalSurname { get; set; } = string.Empty;

        [Required]
        [MaxLength(2000)]
        [DataType(DataType.MultilineText)]
        public string RequestLetter { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [DataType (DataType.Text)]
        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        [MaxLength(100)]
        [DataType(DataType.Text)]
        public string? ReviewedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ReviewedAt { get; set; }
    }
}
