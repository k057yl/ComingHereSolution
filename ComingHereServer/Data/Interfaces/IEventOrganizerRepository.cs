using ComingHereShared.Entities;

namespace ComingHereServer.Data.Interfaces
{
    public interface IEventOrganizerRepository : IRepository<EventOrganizer>
    {
        Task<EventOrganizer?> GetByIdWithEventsAsync(int id);
        Task<bool> UserHasOrganizerAsync(string userId);
    }
}
