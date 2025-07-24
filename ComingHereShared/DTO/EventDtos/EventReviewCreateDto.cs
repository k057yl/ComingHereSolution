namespace ComingHereShared.DTO.EventDtos
{
    public class EventReviewCreateDto
    {
        public int EventId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public string? PhotoUrl { get; set; }
        public int? ParentReviewId { get; set; }
    }
}
