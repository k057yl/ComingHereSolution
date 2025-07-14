using ComingHereShared.Entities;

namespace ComingHereShared.DTO
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Location { get; set; } = "";

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public decimal? Price { get; set; }
        public int? MaxAttendees { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = "";
        public bool IsVip { get; set; }

        public List<EventPhotoDto> Photos { get; set; } = new();

        public static EventDto FromEntity(Event ev, string culture)
        {
            return new EventDto
            {
                Id = ev.Id,
                Name = ev.Name.Get(culture),
                Description = ev.Description.Get(culture),
                Location = ev.Location.Get(culture),
                StartTime = ev.StartTime,
                EndTime = ev.EndTime,
                Latitude = ev.Latitude,
                Longitude = ev.Longitude,
                Price = ev.Price,
                MaxAttendees = ev.MaxAttendees,
                CategoryId = ev.CategoryId,
                CategoryName = ev.Category?.Name ?? "-",
                IsVip = ev.IsVip,
                Photos = ev.Photos.Select(p => new EventPhotoDto { Id = p.Id, PhotoUrl = p.PhotoUrl }).ToList()
            };
        }
    }
}
