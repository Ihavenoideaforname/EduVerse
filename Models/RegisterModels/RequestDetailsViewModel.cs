namespace EduVerse.Models
{
    public class RequestDetailsViewModel
    {
        public Guid Id { get; set; }

        public string SchoolName { get; set; } = string.Empty;
        public string SchoolNameShortcut { get; set; } = string.Empty;
        public string SchoolEmail { get; set; } = string.Empty;
        public string SchoolPhoneNumber { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;
        public string BuildingNumber { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public string PrincipalName { get; set; } = string.Empty;
        public string PrincipalSurname { get; set; } = string.Empty;

        public string RequestLetter { get; set; } = string.Empty;

        public DateTime RequestedAt { get; set; }
        public string Status { get; set; } = string.Empty;

        public string? ReviewedBy { get; set; }
        public DateTime? ReviewedAt { get; set; }

        public string StatusClass =>
            Status.ToLower() switch
            {
                "pending" => "bg-warning text-dark",
                "approved" => "bg-success text-white",
                "rejected" => "bg-danger text-white",
                _ => "bg-secondary text-white"
            };
    }
}
