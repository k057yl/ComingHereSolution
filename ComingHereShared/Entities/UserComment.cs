namespace ComingHereShared.Entities
{
    public class UserComment
    {
        public int Id { get; set; }

        public string TargetUserId { get; set; } = null!;
        public ApplicationUser TargetUser { get; set; } = null!;

        public string AuthorId { get; set; } = null!;
        public ApplicationUser Author { get; set; } = null!;

        public string Comment { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
