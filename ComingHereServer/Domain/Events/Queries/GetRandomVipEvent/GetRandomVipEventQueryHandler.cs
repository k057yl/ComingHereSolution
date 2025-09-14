using ComingHereServer.Data.Interfaces;
using ComingHereShared.DTO.EventDtos;
using MediatR;

namespace ComingHereServer.Domain.Events.Queries.GetRandomVipEvent
{
    public class GetRandomVipEventQueryHandler : IRequestHandler<GetRandomVipEventQuery, EventDto?>
    {
        private readonly IUnitOfWork _uow;

        public GetRandomVipEventQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<EventDto?> Handle(GetRandomVipEventQuery request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var vipEvents = await _uow.Events.GetVipEventsAsync(now);

            if (!vipEvents.Any())
                return null;

            var random = vipEvents[new Random().Next(vipEvents.Count)];
            return EventDto.FromEntity(random, request.Culture);
        }
    }
}
