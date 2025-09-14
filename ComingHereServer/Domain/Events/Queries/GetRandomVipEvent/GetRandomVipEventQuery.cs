using ComingHereShared.DTO.EventDtos;
using MediatR;

namespace ComingHereServer.Domain.Events.Queries.GetRandomVipEvent
{
    public sealed record GetRandomVipEventQuery(string Culture = "uk") : IRequest<EventDto?>;
}
