namespace Talabat.APIs.DTOs
{
    public class TaskWithAttach
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public string UploadedAttachment { get; set; }

        public string DeveloperId { get; set; }

        public string DeveloperName { get; set; }
        public string ProjectName { get; set; }
        public ICollection<CommentDto> comments { get; set; } = new HashSet<CommentDto>();
    }
}
