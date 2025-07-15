using ComingHereServer.Data;
using ComingHereShared.DTO.OrganizerDtos;
using ComingHereShared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComingHereServer.Controllers;

[ApiController]
[Route("api/eventorganizers")]
[Authorize(Roles = "Gala")]
public class EventOrganizersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public EventOrganizersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Create(EventOrganizerCreateDto dto)
    {
        if (!string.IsNullOrEmpty(dto.ApplicationUserId))
        {
            var user = await _userManager.FindByIdAsync(dto.ApplicationUserId);
            if (user == null)
                return BadRequest("Пользователь не найден");
        }

        else
        {
            dto.ApplicationUserId = null;
        }

        var organizer = new EventOrganizer
        {
            ApplicationUserId = dto.ApplicationUserId,
            Name = dto.Name,
            Description = dto.Description,
            Address = dto.Address,
            Phone = dto.Phone,
            Email = dto.Email,
            Website = dto.Website,
            Telegram = dto.Telegram,
            Instagram = dto.Instagram
        };

        _context.EventOrganizers.Add(organizer);
        await _context.SaveChangesAsync();

        return Ok(new { organizer.Id });
    }

    [HttpGet]
    public async Task<ActionResult<List<EventOrganizerDto>>> GetAll()
    {
        var organizers = await _context.EventOrganizers
            .ToListAsync();

        var result = organizers.Select(o => new EventOrganizerDto
        {
            Id = o.Id,
            ApplicationUserId = o.ApplicationUserId,
            Name = o.Name,
            Description = o.Description,
            Address = o.Address,
            Phone = o.Phone,
            Email = o.Email,
            Website = o.Website,
            Telegram = o.Telegram,
            Instagram = o.Instagram
        }).ToList();

        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, EventOrganizerCreateDto dto)
    {
        var organizer = await _context.EventOrganizers.FindAsync(id);
        if (organizer == null)
            return NotFound("Организатор не найден");

        organizer.Name = dto.Name;
        organizer.Description = dto.Description;
        organizer.Address = dto.Address;
        organizer.Phone = dto.Phone;
        organizer.Email = dto.Email;
        organizer.Website = dto.Website;
        organizer.Telegram = dto.Telegram;
        organizer.Instagram = dto.Instagram;

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var organizer = await _context.EventOrganizers
            .Include(o => o.Events)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (organizer == null)
            return NotFound("Организатор не найден");

        if (organizer.Events.Any())
            return BadRequest("Нельзя удалить организатора, у которого есть события");

        _context.EventOrganizers.Remove(organizer);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}