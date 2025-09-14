using ComingHereServer.Data.Interfaces;
using ComingHereShared.DTO.EventDtos;
using MediatR;

namespace ComingHereServer.Domain.Events.Queries.GetActiveEvent
{
    public class GetActiveEventsQueryHandler : IRequestHandler<GetActiveEventsQuery, List<EventDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetActiveEventsQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<EventDto>> Handle(GetActiveEventsQuery request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var events = await _uow.Events.GetActiveWithDetailsAsync(now);
            return events.Select(ev => EventDto.FromEntity(ev, request.Culture)).ToList();
        }
    }
}
