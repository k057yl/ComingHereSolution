using ComingHereShared.Entities;
using System.ComponentModel.DataAnnotations;

namespace ComingHereShared.DTO.EventDtos
{
    public class EventDetailsCreateDto
    {
        public LocalizedString Address { get; set; } = new();

        [Range(-90, 90)]
        public double Latitude { get; set; }

        [Range(-180, 180)]
        public double Longitude { get; set; }

        [Range(0, 100000)]
        public decimal? Price { get; set; }

        [Range(1, 10000)]
        public int? MaxAttendees { get; set; }

        public EventContactInfoCreateDto ContactInfo { get; set; } = new();
    }
}
