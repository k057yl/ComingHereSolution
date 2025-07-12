using ComingHereServer.Data;
using ComingHereShared.Entities;
using ComingHereShared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/eventparticipant")]
[Authorize(Roles = "Gala")]
public class EventParticipantsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EventParticipantsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(EventParticipantCreateDto dto)
    {
        if (dto.EventId > 0)
        {
            var ev = await _context.Events.FindAsync(dto.EventId);
            if (ev == null)
                return NotFound("Событие не найдено");
        }

        var participant = new EventParticipant
        {
            EventId = dto.EventId,
            Name = dto.Name,
            Role = dto.Role,
            Bio = dto.Bio,
            Instagram = dto.Instagram,
            Website = dto.Website,
            Contact = dto.Contact,
            Order = dto.Order
        };

        _context.EventParticipants.Add(participant);
        await _context.SaveChangesAsync();

        return Ok(new { participant.Id });
    }

    [HttpGet("by-event/{eventId:int}")]
    public async Task<IActionResult> GetByEvent(int eventId)
    {
        var participants = await _context.EventParticipants
            .Where(p => p.EventId == eventId)
            .OrderBy(p => p.Order)
            .ToListAsync();

        return Ok(participants);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var participant = await _context.EventParticipants.FindAsync(id);
        if (participant == null)
            return NotFound();

        _context.EventParticipants.Remove(participant);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, EventParticipantCreateDto dto)
    {
        var participant = await _context.EventParticipants.FindAsync(id);
        if (participant == null)
            return NotFound();

        participant.Name = dto.Name;
        participant.Role = dto.Role;
        participant.Bio = dto.Bio;
        participant.Instagram = dto.Instagram;
        participant.Website = dto.Website;
        participant.Contact = dto.Contact;
        participant.Order = dto.Order;

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var participants = await _context.EventParticipants
            .OrderBy(p => p.Order)
            .ToListAsync();

        return Ok(participants);
    }
}