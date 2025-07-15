using ComingHereShared.Entities;

namespace ComingHereShared.DTO.EventDtos
{
    public class EventParticipantCreateDto
    {
        public int? EventId { get; set; }
        public LocalizedString Name { get; set; } = new();
        public LocalizedString Role { get; set; } = new();
        public string? Bio { get; set; }
        public string? Instagram { get; set; }
        public string? Website { get; set; }
        public string? Contact { get; set; }
        public int Order { get; set; }
        public int? CategoryId { get; set; }
    }
}
