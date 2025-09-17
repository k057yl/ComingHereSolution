using ComingHereShared.Entities;
using System.ComponentModel.DataAnnotations;

namespace ComingHereShared.DTO.EventDtos
{
    public class EventCreateDto
    {
        [Required(ErrorMessage = "Cannot be empty")]
        public LocalizedString Name { get; set; } = new();
        [Required(ErrorMessage = "Cannot be empty")]
        public LocalizedString Description { get; set; } = new();
        [Required(ErrorMessage = "Cannot be empty")]
        public LocalizedString Location { get; set; } = new();

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [Range(-90, 90)]
        public double Latitude { get; set; }

        [Range(-180, 180)]
        public double Longitude { get; set; }

        // Категория и организатор
        //[Required(ErrorMessage = "Category cannot be empty")]
        public int CategoryId { get; set; }
        //[Required(ErrorMessage = "Organizer cannot be empty")]
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