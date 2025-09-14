using MediatR;

namespace ComingHereServer.Domain.Events.Commands.DeleteEvent
{
    public sealed record DeleteEventCommand(int EventId, string UserId) : IRequest;
}
