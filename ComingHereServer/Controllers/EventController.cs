using ComingHereServer.Data;
using ComingHereShared.DTO;
using ComingHereShared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComingHereServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Gala")]
        [HttpPost]
        public async Task<IActionResult> Create(EventCreateDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized("Пользователь не найден. Проверь, есть ли claim 'sub' в JWT.");

            var categoryExists = await _context.EventCategories.AnyAsync(c => c.Id == dto.CategoryId);
            if (!categoryExists)
                return BadRequest("Указанная категория не существует.");

            var newEvent = new Event
            {
                Name = dto.Name,
                Description = dto.Description,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Location = dto.Location,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Price = dto.Price,
                MaxAttendees = dto.MaxAttendees,
                CategoryId = dto.CategoryId,
                OrganizerId = user.Id
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return Ok(new { newEvent.Id });
        }

        [HttpPost("{eventId}/upload-photo")]
        [Authorize]
        public async Task<IActionResult> UploadPhoto(int eventId, IFormFile photo)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var ev = await _context.Events.FindAsync(eventId);
            if (ev == null)
                return NotFound("Событие не найдено.");

            if (photo == null || photo.Length == 0)
                return BadRequest("Фото не загружено.");

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = $"{eventId}_{photo.FileName}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = System.IO.File.Create(filePath))
                await photo.CopyToAsync(stream);

            var accessiblePath = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

            var photoEntity = new EventPhoto
            {
                EventId = eventId,
                PhotoUrl = accessiblePath
            };

            _context.EventPhotos.Add(photoEntity);
            await _context.SaveChangesAsync();

            return Ok(new { filePath = accessiblePath });
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<EventDto>> GetEvent(int id)
        {
            var ev = await _context.Events
                .Include(e => e.Photos)
                .Include(e => e.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null)
                return NotFound();

            return new EventDto
            {
                Id = ev.Id,
                Name = ev.Name,
                Description = ev.Description,
                StartTime = ev.StartTime,
                EndTime = ev.EndTime,
                Location = ev.Location,
                Latitude = ev.Latitude,
                Longitude = ev.Longitude,
                Price = ev.Price,
                MaxAttendees = ev.MaxAttendees,
                CategoryId = ev.CategoryId,
                CategoryName = ev.Category?.Name ?? "-",
                Photos = ev.Photos.Select(p => new EventPhotoDto
                {
                    Id = p.Id,
                    PhotoUrl = p.PhotoUrl
                }).ToList()
            };
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var ev = await _context.Events.Include(e => e.Photos).FirstOrDefaultAsync(e => e.Id == id);
            if (ev == null) return NotFound();

            if (ev.OrganizerId != user.Id)
                return Forbid("Ты не можешь удалять чужие события");

            _context.EventPhotos.RemoveRange(ev.Photos);
            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateEvent(int id, EventCreateDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var ev = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            if (ev == null) return NotFound();

            if (ev.OrganizerId != user.Id)
                return Forbid("Редактировать можно только свои события");

            ev.Name = dto.Name;
            ev.Description = dto.Description;
            ev.StartTime = dto.StartTime;
            ev.EndTime = dto.EndTime;
            ev.Location = dto.Location;
            ev.Latitude = dto.Latitude;
            ev.Longitude = dto.Longitude;
            ev.Price = dto.Price;
            ev.MaxAttendees = dto.MaxAttendees;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("active")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActiveEvents()
        {
            var now = DateTime.UtcNow;

            var events = await _context.Events
                .Where(e => e.EndTime == null || e.EndTime > now)
                .AsNoTracking()
                .ToListAsync();

            var dtos = events.Select(ev => new EventDto
            {
                Id = ev.Id,
                Name = ev.Name,
                Description = ev.Description,
                Latitude = ev.Latitude,
                Longitude = ev.Longitude
            }).ToList();

            return Ok(dtos);
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _context.Events
                .AsNoTracking()
                .Include(e => e.Photos)
                .Include(e => e.Category)
                .ToListAsync();

            var dtos = events.Select(ev => new EventDto
            {
                Id = ev.Id,
                Name = ev.Name,
                Description = ev.Description,
                StartTime = ev.StartTime,
                EndTime = ev.EndTime,
                Location = ev.Location,
                Latitude = ev.Latitude,
                Longitude = ev.Longitude,
                Price = ev.Price,
                MaxAttendees = ev.MaxAttendees,
                CategoryId = ev.CategoryId,
                CategoryName = ev.Category?.Name ?? "-",
                Photos = ev.Photos.Select(p => new EventPhotoDto
                {
                    Id = p.Id,
                    PhotoUrl = p.PhotoUrl
                }).ToList()
            }).ToList();

            return Ok(dtos);
        }
    }
}
