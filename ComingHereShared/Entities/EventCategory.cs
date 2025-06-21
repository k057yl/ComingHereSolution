﻿namespace ComingHereShared.Entities
{
    public class EventCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
