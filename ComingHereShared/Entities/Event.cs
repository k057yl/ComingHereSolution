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

        // Координаты
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Цена и участники
        public decimal? Price { get; set; }
        public int? MaxAttendees { get; set; }

        // Фото
        public ICollection<EventPhoto> Photos { get; set; } = new List<EventPhoto>();

        // Организатор
        public string OrganizerId { get; set; } = null!;
        public ApplicationUser Organizer { get; set; } = null!;

        // 💬 Название компании / бренда организатора
        public LocalizedString OrganizerDisplayName { get; set; } = new();

        // Участники
        public ICollection<EventAttendee> Attendees { get; set; } = new List<EventAttendee>();

        // Категория
        public int CategoryId { get; set; }
        public EventCategory Category { get; set; } = null!;
    }
}