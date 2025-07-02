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
    public class EventCategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.EventCategories
                .AsNoTracking()
                .Select(c => new EventCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            return Ok(categories);
        }

        [Authorize(Roles = "Gala")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] EventCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Category name is required.");

            var exists = await _context.EventCategories.AnyAsync(c => c.Name == dto.Name);
            if (exists)
                return Conflict("Category with this name already exists.");

            var category = new EventCategory
            {
                Name = dto.Name
            };

            _context.EventCategories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(new { category.Id, category.Name });
        }

        [Authorize(Roles = "Gala")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.EventCategories.FindAsync(id);
            if (category == null) return NotFound();

            _context.EventCategories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
