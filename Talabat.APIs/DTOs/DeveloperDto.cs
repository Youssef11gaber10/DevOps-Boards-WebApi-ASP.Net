namespace Talabat.APIs.DTOs
{
    public class DeveloperDto
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string RoleName   { get; set; }
        public int ProjectId { get; set; }

        public string? ProjectStatus { get; set; }

    }
}
