using ComingHereShared.Entities;
using System.ComponentModel.DataAnnotations;

namespace ComingHereShared.DTO.EventDtos
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

        // Категория и организатор
        public int CategoryId { get; set; }
        public int OrganizerId { get; set; }

        // Участники
        public List<int> ParticipantIds { get; set; } = new();

        public bool IsVip { get; set; } = false;

        // Вложенные сущности
        public EventDetailsDto Details { get; set; } = new();

        public bool IsRecurring { get; set; }

        public List<EventScheduleDto> Schedules { get; set; } = new();
    }
}