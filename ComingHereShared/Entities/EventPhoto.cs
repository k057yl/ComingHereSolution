namespace ComingHereShared.Entities
{
    public class EventPhoto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;
        public string PhotoUrl { get; set; } = null!;
    }
}
