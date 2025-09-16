using ComingHereServer.Data.Interfaces;
using ComingHereShared.Constants;
using ComingHereShared.DTO.EventDtos;
using ComingHereShared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComingHereServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.GALA)]
    public class EventCategoryController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public EventCategoryController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _uow.EventCategories.GetAllAsync();
            var dtos = categories.Select(c => new EventCategoryDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] EventCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Category name is required.");

            var exists = await _uow.EventCategories.AnyAsync(c => c.Name == dto.Name);
            if (exists)
                return Conflict("Category with this name already exists.");

            var category = new EventCategory { Name = dto.Name };
            await _uow.EventCategories.AddAsync(category);
            await _uow.SaveChangesAsync();

            return Ok(new { category.Id, category.Name });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _uow.EventCategories.GetByIdAsync(id);
            if (category == null) return NotFound();

            _uow.EventCategories.Remove(category);
            await _uow.SaveChangesAsync();
            return NoContent();
        }
    }
}