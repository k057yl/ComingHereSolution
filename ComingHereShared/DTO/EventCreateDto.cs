using ComingHereShared.Entities;
using System.ComponentModel.DataAnnotations;

namespace ComingHereShared.DTO
{
    public class EventCreateDto
    {
        public LocalizedString Name { get; set; } = new();
        public LocalizedString Description { get; set; } = new();
        public LocalizedString Location { get; set; } = new();

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [Range(-90, 90)]
        public double Latitude { get; set; }

        [Range(-180, 180)]
        public double Longitude { get; set; }

        [Range(0, 100000)]
        public decimal? Price { get; set; }

        [Range(1, 10000)]
        public int? MaxAttendees { get; set; }
        public int CategoryId { get; set; }

        public int OrganizerId { get; set; }
        public List<int> ParticipantIds { get; set; } = new();
    }
}