namespace EduVerse.Models
{
    public class UserListViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string SchoolRole { get; set; } = string.Empty;
    }
}
