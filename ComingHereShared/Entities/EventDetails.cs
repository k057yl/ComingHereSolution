namespace ComingHereShared.Entities
{
    public class EventDetails
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }

        public LocalizedString Address { get; set; }
        public decimal? Price { get; set; }
        public int? MaxAttendees { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int? ContactInfoId { get; set; }
        public EventContactInfo? ContactInfo { get; set; }
    }
}
