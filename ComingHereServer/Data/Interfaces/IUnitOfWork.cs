using ComingHereShared.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace ComingHereServer.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEventRepository Events { get; }
        IEventCategoryRepository EventCategories { get; }
        IEventOrganizerRepository EventOrganizers { get; }
        IRepository<EventPhoto> EventPhotos { get; }
        IRepository<EventSchedule> EventSchedules { get; }
        IRepository<EventParticipant> EventParticipants { get; }
        IRepository<ApplicationUser> Users { get; }
        IRepository<OrganizerCategory> OrganizerCategories { get; }

        Task<int> SaveChangesAsync();

        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync(IDbContextTransaction transaction);
        Task RollbackTransactionAsync(IDbContextTransaction transaction);
    }
}
