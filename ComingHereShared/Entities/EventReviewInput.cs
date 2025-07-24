namespace ComingHereShared.Entities
{
    public class EventReviewInput
    {
        public int Rating { get; set; } = 5;
        public string? Comment { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
