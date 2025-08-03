namespace ComingHereShared.Entities
{
    public class EventSchedule
    {
        public int Id { get; set; }

        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; } = null!;
    }
}
