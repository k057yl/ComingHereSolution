using ComingHereServer.Data.Interfaces;
using ComingHereShared.Entities;
using Microsoft.EntityFrameworkCore;

namespace ComingHereServer.Data.Repositories
{
    public class EventOrganizerRepository : GenericRepository<EventOrganizer>, IEventOrganizerRepository
    {
        public EventOrganizerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<EventOrganizer?> GetByIdWithEventsAsync(int id)
        {
            return await _context.EventOrganizers
                .Include(o => o.Events)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<bool> UserHasOrganizerAsync(string userId)
        {
            return await _context.EventOrganizers.AnyAsync(o => o.ApplicationUserId == userId);
        }
    }
}
