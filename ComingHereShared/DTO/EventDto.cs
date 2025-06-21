namespace ComingHereShared.DTO
{
    public class EventDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public string Location { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public decimal? Price { get; set; }
        public int? MaxAttendees { get; set; }

        public List<EventPhotoDto> Photos { get; set; } = new();

        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}
