using ComingHereServer.Data;
using ComingHereShared.DTO.EventDtos;
using ComingHereShared.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ComingHereServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EventReviewController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview(EventReviewCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var exists = await _context.EventReviews.AnyAsync(r =>
                r.EventId == dto.EventId && r.UserId == userId);

            if (exists)
                return BadRequest("Вы уже оставили отзыв для этого события.");

            var evt = await _context.Events.FindAsync(dto.EventId);
            if (evt == null || (!evt.IsRecurring && (evt.EndTime == null || evt.EndTime > DateTime.UtcNow)))
                return BadRequest("Нельзя оставить отзыв до завершения события.");

            var attended = await _context.EventAttendees.AnyAsync(a =>
                a.EventId == dto.EventId && a.UserId == userId);

            if (!attended)
            {
                _context.EventAttendees.Add(new EventAttendee
                {
                    EventId = dto.EventId,
                    UserId = userId!
                });
            }

            var review = new EventReview
            {
                EventId = dto.EventId,
                UserId = userId!,
                Rating = dto.Rating,
                Comment = dto.Comment,
                PhotoUrl = dto.PhotoUrl,
                CreatedAt = DateTime.UtcNow
            };

            _context.EventReviews.Add(review);

            var user = await _context.Users.FindAsync(userId);
            if (user != null)
                user.ReputationPoints += 10;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{eventId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviews(int eventId)
        {
            var reviews = await _context.EventReviews
                .Where(r => r.EventId == eventId)
                .Include(r => r.User)
                .Select(r => new EventReviewDto
                {
                    Rating = r.Rating,
                    Comment = r.Comment,
                    PhotoUrl = r.PhotoUrl,
                    AuthorName = r.User.UserName,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();

            return Ok(reviews);
        }

        [HttpPost("reply")]
        public async Task<IActionResult> CreateReply(EventReviewCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Проверяем, что ParentReviewId указан и существует
            if (dto.ParentReviewId == null)
                return BadRequest("ParentReviewId должен быть указан для ответа.");

            var parentReview = await _context.EventReviews.FindAsync(dto.ParentReviewId);
            if (parentReview == null)
                return BadRequest("Родительский отзыв не найден.");

            var evt = await _context.Events.FindAsync(dto.EventId);
            if (evt == null || evt.EndTime is null || evt.EndTime > DateTime.UtcNow)
                return BadRequest("Нельзя оставить отзыв до завершения события.");

            var review = new EventReview
            {
                EventId = dto.EventId,
                UserId = userId!,
                Rating = dto.Rating,
                Comment = dto.Comment,
                PhotoUrl = dto.PhotoUrl,
                CreatedAt = DateTime.UtcNow,
                ParentReviewId = dto.ParentReviewId
            };

            _context.EventReviews.Add(review);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("hierarchy/{eventId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviewsHierarchy(int eventId)
        {
            var reviews = await _context.EventReviews
                .Where(r => r.EventId == eventId && r.ParentReviewId == null)
                .Include(r => r.User)
                .Include(r => r.Replies)
                    .ThenInclude(reply => reply.User)
                .ToListAsync();

            var result = reviews.Select(r => new EventReviewDtoWithReplies
            {
                Id = r.Id,
                Rating = r.Rating,
                Comment = r.Comment,
                PhotoUrl = r.PhotoUrl,
                AuthorName = r.User.UserName,
                CreatedAt = r.CreatedAt,
                ParentReviewId = r.ParentReviewId,
                Replies = r.Replies.Select(reply => new EventReviewDtoWithReplies
                {
                    Id = reply.Id,
                    Rating = reply.Rating,
                    Comment = reply.Comment,
                    PhotoUrl = reply.PhotoUrl,
                    AuthorName = reply.User.UserName,
                    CreatedAt = reply.CreatedAt,
                    ParentReviewId = reply.ParentReviewId,
                    Replies = new List<EventReviewDtoWithReplies>()
                }).ToList()
            });

            return Ok(result.ToList());
        }
    }
}