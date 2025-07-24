namespace ComingHereShared.DTO.EventDtos
{
    public class EventReviewReplyDto
    {
        public int Id { get; set; }
        public string Comment { get; set; } = "";
        public string AuthorName { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}
