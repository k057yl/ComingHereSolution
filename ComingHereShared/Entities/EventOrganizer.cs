namespace ComingHereShared.Entities
{
    public class EventOrganizer
    {
        public int Id { get; set; }

        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        // Название / бренд / организация
        public LocalizedString Name { get; set; } = new();
        public LocalizedString Description { get; set; } = new();
        public LocalizedString Address { get; set; } = new();

        // Контакты
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? Telegram { get; set; }
        public string? Instagram { get; set; }

        // События, организованные этим организатором
        public ICollection<Event> Events { get; set; } = new List<Event>();

        //Категория организатора
        public int? CategoryId { get; set; }
        public OrganizerCategory? Category { get; set; }
    }
}
