using ComingHereServer.Data.Interfaces;
using ComingHereServer.Data.Repositories;
using ComingHereShared.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace ComingHereServer.Data.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private IEventRepository? _eventRepository;
        private IRepository<EventPhoto>? _eventPhotoRepository;
        private IRepository<EventSchedule>? _eventScheduleRepository;
        private IRepository<EventParticipant>? _eventParticipantRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEventRepository Events => _eventRepository ??= new EventRepository(_context);
        public IRepository<EventPhoto> EventPhotos => _eventPhotoRepository ??= new GenericRepository<EventPhoto>(_context);
        public IRepository<EventSchedule> EventSchedules => _eventScheduleRepository ??= new GenericRepository<EventSchedule>(_context);
        public IRepository<EventParticipant> EventParticipants => _eventParticipantRepository ??= new GenericRepository<EventParticipant>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            await transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync(IDbContextTransaction transaction)
        {
            await transaction.RollbackAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
