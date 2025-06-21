namespace ComingHereShared.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        private DateTime _startTime;
        public DateTime StartTime
        {
            get => _startTime;
            set => _startTime = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        private DateTime? _endTime;
        public DateTime? EndTime
        {
            get => _endTime;
            set => _endTime = value.HasValue
                ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
                : (DateTime?)null;
        }

        public string Location { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public decimal? Price { get; set; }
        public int? MaxAttendees { get; set; }

        public ICollection<EventPhoto> Photos { get; set; } = new List<EventPhoto>();

        public string OrganizerId { get; set; } = null!;
        public ApplicationUser Organizer { get; set; } = null!;

        public ICollection<EventAttendee> Attendees { get; set; } = new List<EventAttendee>();

        public int CategoryId { get; set; }
        public EventCategory Category { get; set; } = null!;
    }
}