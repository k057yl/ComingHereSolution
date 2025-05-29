namespace ComingHereShared.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Location { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public decimal? Price { get; set; } // null — бесплатно
        public int? MaxAttendees { get; set; }

        public string OrganizerId { get; set; } = null!;
        public ApplicationUser Organizer { get; set; } = null!;

        public ICollection<EventAttendee> Attendees { get; set; } = new List<EventAttendee>();
    }
}
