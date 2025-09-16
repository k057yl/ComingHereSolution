using ComingHereShared.Entities;
using System.Linq.Expressions;

namespace ComingHereServer.Data.Interfaces
{
    public interface IEventCategoryRepository : IRepository<EventCategory>
    {
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> AnyAsync(Expression<Func<EventCategory, bool>> predicate);
    }
}
