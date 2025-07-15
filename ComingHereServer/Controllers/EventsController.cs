using ComingHereServer.Data;
using ComingHereShared.DTO.EventDtos;
using ComingHereShared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComingHereServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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
                return Unauthorized("Пользователь не найден.");

            var organizerExists = await _context.EventOrganizers.AnyAsync(o => o.Id == dto.OrganizerId);
            if (!organizerExists)
                return BadRequest("Указанный организатор не существует.");

            var newEvent = new Event
            {
                Name = dto.Name,
                Description = dto.Description,
                Location = dto.Location,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                CategoryId = dto.CategoryId,
                OrganizerId = dto.OrganizerId,
                IsVip = dto.IsVip,
                Details = new EventDetails
                {
                    Address = dto.Details.Address,
                    Price = dto.Details.Price,
                    MaxAttendees = dto.Details.MaxAttendees,
                    Latitude = dto.Details.Latitude,
                    Longitude = dto.Details.Longitude,
                    ContactInfo = new EventContactInfo
                    {
                        Phone = dto.Details.ContactInfo.Phone,
                        Email = dto.Details.ContactInfo.Email,
                        Website = dto.Details.ContactInfo.Website,
                        Telegram = dto.Details.ContactInfo.Telegram,
                        Instagram = dto.Details.ContactInfo.Instagram
                    }
                }
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            if (dto.ParticipantIds != null && dto.ParticipantIds.Any())
            {
                var existingParticipants = await _context.EventParticipants
                    .Where(p => dto.ParticipantIds.Contains(p.Id))
                    .ToListAsync();

                foreach (var participant in existingParticipants)
                {
                    participant.EventId = newEvent.Id;
                }

                await _context.SaveChangesAsync();
            }

            return Ok(new { newEvent.Id });
        }

        [Authorize]
        [HttpPost("{eventId}/upload-photo")]
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

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<EventDto>> GetEvent(int id, [FromQuery] string culture = "uk")
        {
            var ev = await _context.Events
                .Include(e => e.Photos)
                .Include(e => e.Category)
                .Include(e => e.Details)
                .ThenInclude(d => d.ContactInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null)
                return NotFound();

            return EventDto.FromEntity(ev, culture);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var ev = await _context.Events
                .Include(e => e.Photos)
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null) return NotFound();

            if (ev.Organizer.ApplicationUserId != user.Id)
                return Forbid("Ты не можешь удалять чужие события");

            _context.EventPhotos.RemoveRange(ev.Photos);
            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        /*
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEvent(int id, EventCreateDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var ev = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            if (ev == null) return NotFound();

            if (ev.Organizer.ApplicationUserId != user.Id)
                return Forbid("Редактировать можно только свои события");

            ev.Name = dto.Name;
            ev.Description = dto.Description;
            ev.Location = dto.Location;

            ev.StartTime = dto.StartTime;
            ev.EndTime = dto.EndTime;
            ev.Latitude = dto.Latitude;
            ev.Longitude = dto.Longitude;
            ev.Price = dto.Price;
            ev.MaxAttendees = dto.MaxAttendees;

            await _context.SaveChangesAsync();

            return Ok();
        }
        */
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllEvents([FromQuery] string culture = "uk")
        {
            var events = await _context.Events
                .AsNoTracking()
                .Include(e => e.Photos)
                .Include(e => e.Category)
                .Include(e => e.Details)
                    .ThenInclude(d => d.ContactInfo)
                .Include(e => e.Organizer)
                .ToListAsync();

            var dtos = events.Select(ev => EventDto.FromEntity(ev, culture)).ToList();

            return Ok(dtos);
        }

        [AllowAnonymous]
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveEvents([FromQuery] string culture = "uk")
        {
            var now = DateTime.UtcNow;

            var events = await _context.Events
                .Where(e => e.EndTime == null || e.EndTime > now)
                .AsNoTracking()
                .Include(e => e.Photos)
                .Include(e => e.Category)
                .Include(e => e.Details)
                .ThenInclude(d => d.ContactInfo)
                .ToListAsync();

            var dtos = events.Select(ev => EventDto.FromEntity(ev, culture)).ToList();

            return Ok(dtos);
        }

        [AllowAnonymous]
        [HttpGet("vip-random")]
        public async Task<IActionResult> GetRandomVipEvent([FromQuery] string culture = "uk")
        {
            var vipEvents = await _context.Events
                .Where(e => e.IsVip)
                .Include(e => e.Category)
                .Include(e => e.Details)
                    .ThenInclude(d => d.ContactInfo)
                .Include(e => e.Organizer)
                .Include(e => e.Photos)
                .ToListAsync();

            if (!vipEvents.Any())
                return NotFound();

            var random = vipEvents[new Random().Next(vipEvents.Count)];
            return Ok(EventDto.FromEntity(random, culture));
        }
    }
}