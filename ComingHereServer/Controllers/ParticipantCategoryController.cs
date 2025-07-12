using ComingHereServer.Data;
using ComingHereShared.DTO;
using ComingHereShared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComingHereServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantCategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ParticipantCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.ParticipantCategories
                .AsNoTracking()
                .Select(c => new ParticipantCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            return Ok(categories);
        }

        [Authorize(Roles = "Gala")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] ParticipantCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Category name is required.");

            var exists = await _context.ParticipantCategories.AnyAsync(c => c.Name == dto.Name);
            if (exists)
                return Conflict("Category already exists.");

            var category = new ParticipantCategory
            {
                Name = dto.Name
            };

            _context.ParticipantCategories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(new { category.Id, category.Name });
        }

        [Authorize(Roles = "Gala")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.ParticipantCategories.FindAsync(id);
            if (category == null) return NotFound();

            _context.ParticipantCategories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}