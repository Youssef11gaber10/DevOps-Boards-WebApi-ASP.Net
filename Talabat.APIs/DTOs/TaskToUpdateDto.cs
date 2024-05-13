namespace Talabat.APIs.DTOs
{
    public class TaskToUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int ProjectId { get; set; }
        public string? Status { get; set; }//those must not sent but we will keep them until make them null in db in next migrations
        public string? DeveloperId { get; set; }//those must
        
        public string? DeveloperName { get; set; }//those must
    }
}
