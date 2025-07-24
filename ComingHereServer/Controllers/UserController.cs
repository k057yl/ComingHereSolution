using ComingHereServer.Data;
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
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfile(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var comments = await _context.UserComments
                .Where(c => c.TargetUserId == id)
                .Select(c => new
                {
                    c.Author.UserName,
                    c.Comment,
                    c.CreatedAt
                })
                .ToListAsync();

            return Ok(new
            {
                user.Id,
                user.UserName,
                Reputation = user.ReputationPoints,
                Rank = ReputationHelper.GetRank(user.ReputationPoints),
                Comments = comments
            });
        }

        [HttpPost("{id}/comment")]
        [Authorize]
        public async Task<IActionResult> AddComment(string id, [FromBody] string comment)
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id == authorId)
                return BadRequest("Нельзя комментировать себя.");

            var userExists = await _context.Users.AnyAsync(u => u.Id == id);
            if (!userExists) return NotFound();

            _context.UserComments.Add(new UserComment
            {
                TargetUserId = id,
                AuthorId = authorId!,
                Comment = comment,
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
