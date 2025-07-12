namespace ComingHereShared.Entities
{
    public class ParticipantCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<EventParticipant> Participants { get; set; } = new List<EventParticipant>();
    }
}
