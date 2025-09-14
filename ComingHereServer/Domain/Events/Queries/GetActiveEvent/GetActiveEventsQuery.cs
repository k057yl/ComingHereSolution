using ComingHereShared.DTO.EventDtos;
using MediatR;

namespace ComingHereServer.Domain.Events.Queries.GetActiveEvent
{
    public sealed record GetActiveEventsQuery(string Culture = "uk") : IRequest<List<EventDto>>;
}
