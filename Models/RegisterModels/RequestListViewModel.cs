namespace EduVerse.Models
{
    public class RequestListViewModel
    {
        public Guid Id { get; set; }
        public string SchoolName { get; set; } = string.Empty;
        public string PrincipalName {  get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public string RequestStatus { get; set; } = string.Empty;

        public string StatusClass =>
            RequestStatus.ToLower() switch
            {
                "pending" => "bg-warning text-dark",
                "approved" => "bg-success text-white",
                "rejected" => "bg-danger text-white",
                _ => "bg-secondary text-white"
            };
    }
}
