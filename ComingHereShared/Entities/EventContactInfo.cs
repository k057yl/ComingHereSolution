namespace ComingHereShared.Entities
{
    public class EventContactInfo
    {
        public int Id { get; set; }
        public int EventDetailsId { get; set; }
        public EventDetails EventDetails { get; set; }

        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? Telegram { get; set; }
        public string? Instagram { get; set; }
    }
}
