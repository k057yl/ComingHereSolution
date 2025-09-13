using ComingHereServer.Data.Interfaces;
using ComingHereShared.Entities;
using Microsoft.EntityFrameworkCore;

namespace ComingHereServer.Data.Repositories
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> OrganizerExists(int organizerId)
        {
            return await _context.EventOrganizers.AnyAsync(o => o.Id == organizerId);
        }

        public async Task<Event?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Events
                .Include(e => e.Photos)
                .Include(e => e.Category)
                .Include(e => e.Details)
                    .ThenInclude(d => d.ContactInfo)
                .Include(e => e.Schedules)
                .Include(e => e.Organizer)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<Event>> GetAllWithDetailsAsync()
        {
            return await _context.Events
                .Include(e => e.Photos)
                .Include(e => e.Category)
                .Include(e => e.Details)
                    .ThenInclude(d => d.ContactInfo)
                .Include(e => e.Schedules)
                .Include(e => e.Organizer)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Event>> GetActiveWithDetailsAsync(DateTime now)
        {
            return await _context.Events
                .Where(e => e.EndTime == null || e.EndTime > now)
                .Include(e => e.Photos)
                .Include(e => e.Category)
                .Include(e => e.Details)
                    .ThenInclude(d => d.ContactInfo)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Event>> GetVipEventsAsync(DateTime now)
        {
            return await _context.Events
                .Where(e => e.IsVip && e.StartTime > now)
                .Include(e => e.Photos)
                .Include(e => e.Category)
                .Include(e => e.Details)
                    .ThenInclude(d => d.ContactInfo)
                .Include(e => e.Organizer)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
