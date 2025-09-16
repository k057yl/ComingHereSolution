using ComingHereServer.Data.Interfaces;
using ComingHereShared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ComingHereServer.Data.Repositories
{
    public class EventCategoryRepository : GenericRepository<EventCategory>, IEventCategoryRepository
    {
        public EventCategoryRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Set<EventCategory>().AnyAsync(c => c.Name == name);
        }

        public async Task<bool> AnyAsync(Expression<Func<EventCategory, bool>> predicate)
        {
            return await _context.EventCategories.AnyAsync(predicate);
        }
    }
}
