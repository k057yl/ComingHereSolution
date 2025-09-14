using ComingHereServer.Data.Interfaces;
using ComingHereServer.Domain.Events.Commands.CreateEvent;
using ComingHereServer.Domain.Events.Commands.DeleteEvent;
using ComingHereServer.Domain.Events.Commands.UpdateEvent;
using ComingHereServer.Domain.Events.Commands.UploadPhoto;
using ComingHereServer.Domain.Events.Queries.GetActiveEvent;
using ComingHereServer.Domain.Events.Queries.GetAllEvent;
using ComingHereServer.Domain.Events.Queries.GetRandomVipEvent;
using ComingHereShared.Constants;
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

        public EventsController(IUnitOfWork uow, IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _uow = uow;
            _mediator = mediator;
            _userManager = userManager;
        }

        [Authorize(Roles = Roles.GALA)]
        [HttpPost]
        public async Task<IActionResult> Create(EventCreateDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var command = new CreateEventCommand(dto, user.Id);
            var id = await _mediator.Send(command);

            return Ok(new { id });
        }

        [Authorize]
        [HttpPost("{eventId}/upload-photo")]
        public async Task<IActionResult> UploadPhoto(int eventId, IFormFile photo)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var cmd = new UploadPhotoCommand(eventId, photo, user.Id);
            var path = await _mediator.Send(cmd);
            return Ok(new { filePath = path });
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<EventDto>> GetEvent(int id, [FromQuery] string culture = "uk")
        {
            var ev = await _uow.Events.GetByIdWithDetailsAsync(id);
            if (ev == null) return NotFound();

            return EventDto.FromEntity(ev, culture);
        }

        [Authorize(Roles = Roles.GALA)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            await _mediator.Send(new DeleteEventCommand(id, user.Id));
            return NoContent();
        }

        [Authorize(Roles = Roles.GALA)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventCreateDto dto)
        {
            var userId = User?.FindFirst("sub")?.Value ?? "";
            var result = await _mediator.Send(new UpdateEventCommand(id, dto, userId));
            return result ? NoContent() : NotFound();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllEvents([FromQuery] string culture = "uk")
        {
            var dtos = await _mediator.Send(new GetAllEventsQuery(culture));
            return Ok(dtos);
        }

        [AllowAnonymous]
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveEvents([FromQuery] string culture = "uk")
        {
            var dtos = await _mediator.Send(new GetActiveEventsQuery(culture));
            return Ok(dtos);
        }

        [AllowAnonymous]
        [HttpGet("vip-random")]
        public async Task<IActionResult> GetRandomVipEvent([FromQuery] string culture = "uk")
        {
            var dto = await _mediator.Send(new GetRandomVipEventQuery(culture));
            if (dto == null) return NoContent();
            return Ok(dto);
        }
    }
}