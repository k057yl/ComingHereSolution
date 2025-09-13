using ComingHereShared.DTO.EventDtos;
using MediatR;

namespace ComingHereServer.Domain.Events.Commands.CreateEvent
{
    public class CreateEventCommand : IRequest<int>
    {
        public EventCreateDto Dto { get; set; }
        public string UserId { get; set; }

        public CreateEventCommand(EventCreateDto dto, string userId)
        {
            Dto = dto;
            UserId = userId;
        }
    }
}
