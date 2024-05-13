namespace Talabat.APIs.DTOs
{
    public class CreateComment
    {

        //public string Id { get; set; }
        public string Text { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public int TaskId { get; set; }
    }
}
