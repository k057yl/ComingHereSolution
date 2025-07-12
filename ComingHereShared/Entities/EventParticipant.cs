namespace ComingHereShared.Entities
{
    public class EventParticipant
    {
        public int Id { get; set; }

        // Привязка к событию
        public int? EventId { get; set; }
        public Event? Event { get; set; }

        // Имя и роль участника (музыкант, лектор, фокусник, инфоцыган...)
        public LocalizedString Name { get; set; } = new();
        public LocalizedString Role { get; set; } = new();

        // Биография / краткая инфа
        public string? Bio { get; set; }

        // Контакты и ссылки
        public string? Instagram { get; set; }
        public string? Website { get; set; }
        public string? Contact { get; set; }

        // Порядок отображения (если нужно на фронте отсортировать)
        public int Order { get; set; }

        // Категория участника
        public int? CategoryId { get; set; }
        public ParticipantCategory? Category { get; set; }
    }
}
