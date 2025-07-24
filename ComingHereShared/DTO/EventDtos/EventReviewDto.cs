namespace ComingHereShared.DTO.EventDtos
{
    public class EventReviewDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public string? PhotoUrl { get; set; }
        public string AuthorName { get; set; } = "";
        public DateTime CreatedAt { get; set; }

        public List<EventReviewReplyDto> Replies { get; set; } = new();
        public string? NewComment { get; set; }
        public int? ParentReviewId { get; set; }
    }
}
