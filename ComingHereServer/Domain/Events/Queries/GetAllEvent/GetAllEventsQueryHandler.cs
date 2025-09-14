using ComingHereServer.Data.Interfaces;
using ComingHereShared.DTO.EventDtos;
using MediatR;

namespace ComingHereServer.Domain.Events.Queries.GetAllEvent
{
    public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQuery, List<EventDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetAllEventsQueryHandler(IUnitOfWork uow) 
        {
            _uow = uow;
        }
        public async Task<List<EventDto>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
        {
            var events = await _uow.Events.GetAllWithDetailsAsync();
            return events.Select(ev => EventDto.FromEntity(ev, request.Culture)).ToList();
        }
    }
}
