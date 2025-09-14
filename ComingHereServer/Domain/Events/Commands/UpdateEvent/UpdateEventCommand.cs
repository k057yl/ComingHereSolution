using ComingHereShared.DTO.EventDtos;
using MediatR;

namespace ComingHereServer.Domain.Events.Commands.UpdateEvent
{
    public record UpdateEventCommand(int EventId, EventCreateDto Dto, string UserId) : IRequest<bool>;
}
