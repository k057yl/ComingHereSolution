using ComingHereShared.DTO.EventDtos;
using MediatR;

namespace ComingHereServer.Domain.Events.Commands.CreateEvent
{
    public record CreateEventCommand(EventCreateDto Dto, string UserId) : IRequest<int>;
}
