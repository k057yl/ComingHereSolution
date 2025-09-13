using ComingHereServer.Data;
using ComingHereServer.Data.Interfaces;
using ComingHereServer.Domain.Events.Commands.CreateEvent;
using ComingHereServer.Interfaces;
using ComingHereServer.Services;
using ComingHereShared.DTO.EventDtos;
using ComingHereShared.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ComingHereServer.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHtmlSanitizingService _sanitizingService;

        public EventsController(IUnitOfWork uow, IMediator mediator, UserManager<ApplicationUser> userManager, IHtmlSanitizingService sanitizingService)
        {
            _uow = uow;
            _mediator = mediator;
            _userManager = userManager;
            _sanitizingService = sanitizingService;
        }

        [Authorize(Roles = "Gala")]
        [HttpPost]
        public async Task<IActionResult> Create(EventCreateDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var command = new CreateEventCommand(dto, user.Id);
            var id = await _mediator.Send(command);

            return Ok(new { id });
        }
        /*
        [Authorize(Roles = "Gala")]
        [HttpPost]
        public async Task<IActionResult> Create(EventCreateDto dto)
        {
            DtoSanitizer.Sanitize(dto, _sanitizingService);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            // Проверка существования организатора
            bool organizerExists = await _uow.Events.OrganizerExists(dto.OrganizerId);
            if (!organizerExists)
                return BadRequest("Указанный организатор не существует.");

            var newEvent = new Event
            {
                Name = dto.Name,
                Description = dto.Description,
                Location = dto.Location,
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
                },
                IsRecurring = dto.IsRecurring
            };

            if (!dto.IsRecurring)
            {
                newEvent.StartTime = dto.StartTime;
                newEvent.EndTime = dto.EndTime;
            }

            await _uow.Events.AddAsync(newEvent);
            await _uow.SaveChangesAsync();

            // Добавляем расписание, если событие повторяющееся
            if (dto.IsRecurring && dto.Schedules != null)
            {
                foreach (var s in dto.Schedules)
                {
                    await _uow.EventSchedules.AddAsync(new EventSchedule
                    {
                        EventId = newEvent.Id,
                        DayOfWeek = s.DayOfWeek,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        StartDate = s.StartDate,
                        EndDate = s.EndDate
                    });
                }

                await _uow.SaveChangesAsync();
            }

            // Привязка участников
            if (dto.ParticipantIds != null)
            {
                var participants = await _uow.EventParticipants.FindAsync(p => dto.ParticipantIds.Contains(p.Id));
                foreach (var participant in participants)
                    participant.EventId = newEvent.Id;

                await _uow.SaveChangesAsync();
            }

            return Ok(new { newEvent.Id });
        }
        */

        [Authorize]
        [HttpPost("{eventId}/upload-photo")]
        public async Task<IActionResult> UploadPhoto(int eventId, IFormFile photo)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var ev = await _uow.Events.GetByIdAsync(eventId);
            if (ev == null) return NotFound("Событие не найдено.");

            if (photo == null || photo.Length == 0)
                return BadRequest("Фото не загружено.");

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = $"{eventId}_{photo.FileName}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using var stream = System.IO.File.Create(filePath);
            await photo.CopyToAsync(stream);

            var accessiblePath = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

            var photoEntity = new EventPhoto
            {
                EventId = eventId,
                PhotoUrl = accessiblePath
            };

            await _uow.EventPhotos.AddAsync(photoEntity);
            await _uow.SaveChangesAsync();

            return Ok(new { filePath = accessiblePath });
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<EventDto>> GetEvent(int id, [FromQuery] string culture = "uk")
        {
            var ev = await _uow.Events.GetByIdWithDetailsAsync(id);
            if (ev == null) return NotFound();

            return EventDto.FromEntity(ev, culture);
        }

        [Authorize(Roles = "Gala")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            // Получаем событие с деталями, включая фото и организатора
            var ev = await _uow.Events.GetByIdWithDetailsAsync(id);
            if (ev == null) return NotFound();

            // Удаляем все фото события
            foreach (var photo in ev.Photos)
                _uow.EventPhotos.Remove(photo);

            // Удаляем само событие
            _uow.Events.Remove(ev);
            await _uow.SaveChangesAsync();

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
            var events = await _uow.Events.GetAllWithDetailsAsync();
            var dtos = events.Select(ev => EventDto.FromEntity(ev, culture)).ToList();
            return Ok(dtos);
        }

        [AllowAnonymous]
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveEvents([FromQuery] string culture = "uk")
        {
            var now = DateTime.UtcNow;
            var events = await _uow.Events.GetActiveWithDetailsAsync(now);
            var dtos = events.Select(ev => EventDto.FromEntity(ev, culture)).ToList();
            return Ok(dtos);
        }

        [AllowAnonymous]
        [HttpGet("vip-random")]
        public async Task<IActionResult> GetRandomVipEvent([FromQuery] string culture = "uk")
        {
            var now = DateTime.UtcNow;
            var vipEvents = await _uow.Events.GetVipEventsAsync(now);

            // Возвращаем 204 NoContent, если нет VIP-событий.
            // Клиент должен учитывать, что ответ пустой.
            // Это позволяет избежать ошибок при обработке пустого списка.
            if (!vipEvents.Any())
                return NoContent();

            var random = vipEvents[new Random().Next(vipEvents.Count)];
            return Ok(EventDto.FromEntity(random, culture));
        }
    }
}