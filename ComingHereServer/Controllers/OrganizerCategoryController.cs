using ComingHereServer.Data;
using ComingHereShared.DTO.OrganizerDtos;
using ComingHereShared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComingHereServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizerCategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrganizerCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.OrganizerCategories
                .AsNoTracking()
                .Select(c => new OrganizerCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            return Ok(categories);
        }

        [Authorize(Roles = "Gala")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] OrganizerCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Category name is required.");

            var exists = await _context.OrganizerCategories.AnyAsync(c => c.Name == dto.Name);
            if (exists)
                return Conflict("Category already exists.");

            var category = new OrganizerCategory
            {
                Name = dto.Name
            };

            _context.OrganizerCategories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(new { category.Id, category.Name });
        }

        [Authorize(Roles = "Gala")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.OrganizerCategories.FindAsync(id);
            if (category == null)
                return NotFound();

            // Проверяем, есть ли организаторы с этой категорией
            var hasOrganizers = await _context.EventOrganizers.AnyAsync(o => o.CategoryId == id);
            if (hasOrganizers)
                return BadRequest("Нельзя удалить категорию, которая используется организаторами.");

            _context.OrganizerCategories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}