using ComingHereShared.Entities;

namespace ComingHereShared.DTO.EventDtos
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Location { get; set; } = "";
        public string Address { get; set; } = "";

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public decimal? Price { get; set; }
        public int? MaxAttendees { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = "";
        public bool IsVip { get; set; }
        public bool IsRecurring { get; set; }
        public string OrganizerName { get; set; } = "";
        public int OrganizerId { get; set; }

        // Контактная информация
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? Telegram { get; set; }
        public string? Instagram { get; set; }

        public List<EventPhotoDto> Photos { get; set; } = new();
        public List<EventReviewDto> Reviews { get; set; } = new();

        public static EventDto FromEntity(Event ev, string culture)
        {
            var details = ev.Details;
            var contact = details?.ContactInfo;

            return new EventDto
            {
                Id = ev.Id,
                Name = ev.Name.Get(culture),
                Description = ev.Description.Get(culture),
                Location = ev.Location.Get(culture),
                Address = details?.Address?.Get(culture) ?? "",
                Latitude = details?.Latitude ?? 0,
                Longitude = details?.Longitude ?? 0,
                Price = details?.Price,
                MaxAttendees = details?.MaxAttendees,
                StartTime = ev.StartTime,
                EndTime = ev.EndTime,
                CategoryId = ev.CategoryId,
                CategoryName = ev.Category?.Name ?? "-",
                IsVip = ev.IsVip,
                IsRecurring = ev.IsRecurring,
                Phone = contact?.Phone ?? "",
                Email = contact?.Email ?? "",
                Website = contact?.Website ?? "",
                Telegram = contact?.Telegram ?? "",
                Instagram = contact?.Instagram ?? "",
                OrganizerId = ev.OrganizerId,
                OrganizerName = ev.Organizer != null
                    ? ev.Organizer.Name.Get(culture)
                    : "N/A",
                Photos = ev.Photos?.Select(p => new EventPhotoDto
                {
                    Id = p.Id,
                    PhotoUrl = p.PhotoUrl
                }).ToList() ?? new List<EventPhotoDto>()
            };
        }
    }
}