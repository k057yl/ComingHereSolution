using ComingHereShared.DTO.EventDtos;
using MediatR;

namespace ComingHereServer.Domain.Events.Queries.GetAllEvent
{
    public sealed record GetAllEventsQuery(string Culture = "uk") : IRequest<List<EventDto>>;
}
