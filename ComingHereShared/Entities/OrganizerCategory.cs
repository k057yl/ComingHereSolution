namespace ComingHereShared.Entities
{
    public class OrganizerCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<EventOrganizer> Organizers { get; set; } = new List<EventOrganizer>();
    }
}
