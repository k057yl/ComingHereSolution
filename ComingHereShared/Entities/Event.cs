namespace ComingHereShared.Entities
{
    public class Event
    {
        public int Id { get; set; }

        // Локализуемые поля
        public LocalizedString Name { get; set; } = new();
        public LocalizedString Description { get; set; } = new();
        public LocalizedString Location { get; set; } = new();

        // Дата и время
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

        // Организатор
        public int OrganizerId { get; set; }
        public EventOrganizer Organizer { get; set; } = null!;

        // Категория
        public int CategoryId { get; set; }
        public EventCategory Category { get; set; } = null!;

        // Детали и контакты
        public EventDetails Details { get; set; } = new();
        public EventContactInfo ContactInfo { get; set; } = new();

        // Фото
        public ICollection<EventPhoto> Photos { get; set; } = new List<EventPhoto>();

        // Участники (музыканты, спикеры и т.п.)
        public ICollection<EventParticipant> Participants { get; set; } = new List<EventParticipant>();

        // Посетители
        public ICollection<EventAttendee> Attendees { get; set; } = new List<EventAttendee>();

        public bool IsVip { get; set; } = false;
    }
}