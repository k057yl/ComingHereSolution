using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ComingHereShared.DTO
{
    public class EventCreateDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [Required]
        public string Location { get; set; } = null!;

        [Range(-90, 90)]
        public double Latitude { get; set; }

        [Range(-180, 180)]
        public double Longitude { get; set; }

        [Range(0, 100000)]
        public decimal? Price { get; set; }

        [Range(1, 10000)]
        public int? MaxAttendees { get; set; }
        public int CategoryId { get; set; }
    }
}
