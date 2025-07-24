using System.ComponentModel.DataAnnotations;

namespace ComingHereShared.Entities
{
    public class EventReview
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        public string? PhotoUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Для поддержки ответов на отзывы:
        public int? ParentReviewId { get; set; }
        public EventReview? ParentReview { get; set; }

        public List<EventReview> Replies { get; set; } = new();
    }
}