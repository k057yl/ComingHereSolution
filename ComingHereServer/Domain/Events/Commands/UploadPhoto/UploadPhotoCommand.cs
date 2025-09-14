using MediatR;

namespace ComingHereServer.Domain.Events.Commands.UploadPhoto
{
    public record UploadPhotoCommand(int EventId, IFormFile Photo, string UserId) : IRequest<string>;
}
