namespace ComingHereShared.DTO.EventDtos
{
    public class EventReviewDtoWithReplies : EventReviewDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string UserId { get; set; } = null!;
        public string AuthorName { get; set; } = null!;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public string? PhotoUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? ParentReviewId { get; set; }

        public List<EventReviewDtoWithReplies> Replies { get; set; } = new();
    }
}
