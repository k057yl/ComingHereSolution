using ComingHereServer.Data;
using ComingHereShared.DTO;
using ComingHereShared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(EventCreateDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized("Пользователь не найден. Проверь, есть ли claim 'sub' в JWT.");

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
                OrganizerId = user.Id
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return Ok(new { newEvent.Id });
        }
    }
}
