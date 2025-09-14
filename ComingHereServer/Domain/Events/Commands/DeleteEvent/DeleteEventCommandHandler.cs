using ComingHereServer.Data.Interfaces;
using MediatR;

namespace ComingHereServer.Domain.Events.Commands.DeleteEvent
{
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
    {
        private readonly IUnitOfWork _uow;

        public DeleteEventCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            // Получаем событие с деталями
            var ev = await _uow.Events.GetByIdWithDetailsAsync(request.EventId);
            if (ev == null)
                throw new KeyNotFoundException("Событие не найдено.");

            // Удаляем фото
            foreach (var photo in ev.Photos)
                _uow.EventPhotos.Remove(photo);

            // Удаляем само событие
            _uow.Events.Remove(ev);
            await _uow.SaveChangesAsync();

            return Unit.Value;
        }
    }
}