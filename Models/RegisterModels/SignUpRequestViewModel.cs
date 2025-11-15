using System.ComponentModel.DataAnnotations;

namespace EduVerse.Models
{
    public class SignUpRequestViewModel
    {
        [Required(ErrorMessage = "School name is required.")]
        [MaxLength(200, ErrorMessage = "School name can't exceed 200 characters.")]
        [RegularExpression(@"^[\p{L}\d\s\-\&]{1,200}$", ErrorMessage = "School name can only contain letters, numbers, spaces and hyphens.")]
        [DataType(DataType.Text)]
        [Display(Name = "School Name")]
        public string SchoolName { get; set; } = string.Empty;

        [Required(ErrorMessage = "School name shortcut is required.")]
        [MaxLength(20, ErrorMessage = "School name shortcut can't exceed 20 characters.")]
        [RegularExpression(@"^[A-Za-z0-9]{1,20}$", ErrorMessage = "School name shortcut can only contain letters and numbers.")]
        [DataType(DataType.Text)]
        [Display(Name = "School Shortcut")]
        public string SchoolNameShortcut { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact email is required.")]
        [MaxLength(100, ErrorMessage = "Contact email can't exceed 100 characters.")]
        [RegularExpression(@"^[\w\.\-]+@([\w\-]+\.)+[A-Za-z]{2,}$", ErrorMessage = "Invalid email format.")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Contact Email")]
        public string SchoolEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact phone number is required.")]
        [MaxLength(20, ErrorMessage = "Contact phone number can't exceed 20 characters.")]
        [RegularExpression(@"^[\d\s\+\-\(\)]{1,20}$", ErrorMessage = "Contact phone number can only contain digits, spaces, +, - and parentheses.")]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Contact Phone Number")]
        public string SchoolPhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Street is required.")]
        [MaxLength(100, ErrorMessage = "Street can't exceed 100 characters.")]
        [RegularExpression(@"^[\p{L}\d\s\.\,\-]{1,100}$", ErrorMessage = "Street can only contain letters, numbers, spaces, periods, commas and hyphens.")]
        [DataType(DataType.Text)]
        public string Street { get; set; } = string.Empty;

        [Required(ErrorMessage = "Building number is required.")]
        [MaxLength(20, ErrorMessage = "Building number can't exceed 20 characters.")]
        [RegularExpression(@"^[\w\d]{1,20}$", ErrorMessage = "Building number can only contain letters and numbers.")]
        [DataType(DataType.Text)]
        [Display(Name = "Building Number")]
        public string BuildingNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Postal code is reqired.")]
        [MaxLength(20, ErrorMessage = "Postal code can't exceed 20 characters.")]
        [RegularExpression(@"^[\d\s\-]{1,20}$", ErrorMessage = "Postal code can only contain numbers, spaces and hyphens.")]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required.")]
        [MaxLength(100, ErrorMessage = "City can't exceed 100 characters.")]
        [RegularExpression(@"^[\p{L}\s\-]{1,100}$", ErrorMessage = "City can only contain letters, spaces and hyphens.")]
        [DataType(DataType.Text)]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required.")]
        [MaxLength(100, ErrorMessage = "State can't exceed 100 characters.")]
        [RegularExpression(@"^[\p{L}\s\-]{1,100}$", ErrorMessage = "State can only contain letters, spaces and hyphens.")]
        [DataType(DataType.Text)]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required.")]
        [MaxLength(100, ErrorMessage = "Country can't exceed 100 characters.")]
        [RegularExpression(@"^[\p{L}\s\-]{1,100}$", ErrorMessage = "Country can only contain letters, spaces and hyphens.")]
        [DataType(DataType.Text)]
        public string Country { get; set; } = string.Empty;

        [Required(ErrorMessage = "Principal name is required.")]
        [MaxLength(100, ErrorMessage = "Principal name can't exceed 100 characters.")]
        [RegularExpression(@"^[\p{L}\s\-]{1,100}$", ErrorMessage = "Principal name can only contain letters, spaces and hyphens.")]
        [DataType(DataType.Text)]
        [Display(Name = "Principal Name")]
        public string PrincipalName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Principal surname is required.")]
        [MaxLength(100, ErrorMessage = "Principal surname can't exceed 100 characters.")]
        [RegularExpression(@"^[\p{L}\s\-]{1,100}$", ErrorMessage = "Principal surname can only contain letters, spaces and hyphens.")]
        [DataType(DataType.Text)]
        [Display(Name = "Principal Surname")]
        public string PrincipalSurname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Request letter is required.")]
        [MaxLength(2000, ErrorMessage = "Request letter can't exceed 100 characters.")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Request Letter")]
        public string RequestLetter { get; set; } = string.Empty;
    }
}
