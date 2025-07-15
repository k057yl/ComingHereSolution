using ComingHereShared.Entities;
using System.ComponentModel.DataAnnotations;

namespace ComingHereShared.DTO.EventDtos
{
    public class EventDetailsDto
    {
        public LocalizedString Address { get; set; } = new();

        [Range(0, 100000)]
        public decimal? Price { get; set; }

        [Range(1, 10000)]
        public int? MaxAttendees { get; set; }

        public EventContactInfoDto ContactInfo { get; set; } = new();

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
