using ComingHereShared.Entities;

namespace ComingHereServer.Data.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<bool> OrganizerExists(int organizerId);
        Task<Event?> GetByIdWithDetailsAsync(int id);
        Task<List<Event>> GetAllWithDetailsAsync();
        Task<List<Event>> GetActiveWithDetailsAsync(DateTime now);
        Task<List<Event>> GetVipEventsAsync(DateTime from);
    }
}
