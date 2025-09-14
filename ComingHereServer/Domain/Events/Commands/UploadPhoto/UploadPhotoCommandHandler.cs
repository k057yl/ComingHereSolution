using ComingHereServer.Data.Interfaces;
using ComingHereShared.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ComingHereServer.Domain.Events.Commands.UploadPhoto
{
    public class UploadPhotoCommandHandler : IRequestHandler<UploadPhotoCommand, string>
    {
        private readonly IUnitOfWork _uow;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public UploadPhotoCommandHandler(IUnitOfWork uow, IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _uow = uow;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<string> Handle(UploadPhotoCommand request, CancellationToken cancellationToken)
        {
            // Получаем пользователя через Identity
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                throw new UnauthorizedAccessException("Пользователь не найден.");

            // Проверяем событие
            var ev = await _uow.Events.GetByIdAsync(request.EventId);
            if (ev == null)
                throw new KeyNotFoundException("Событие не найдено.");

            // Проверяем файл
            if (request.Photo == null || request.Photo.Length == 0)
                throw new ArgumentException("Фото не загружено.");

            // Папка uploads
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = $"{request.EventId}_{request.Photo.FileName}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using var stream = System.IO.File.Create(filePath);
            await request.Photo.CopyToAsync(stream, cancellationToken);

            // Формируем публичный путь
            var requestScheme = _httpContextAccessor.HttpContext?.Request.Scheme ?? "http";
            var requestHost = _httpContextAccessor.HttpContext?.Request.Host.Value ?? "localhost";
            var accessiblePath = $"{requestScheme}://{requestHost}/uploads/{fileName}";

            // Сохраняем в базе
            var photoEntity = new EventPhoto
            {
                EventId = request.EventId,
                PhotoUrl = accessiblePath
            };
            await _uow.EventPhotos.AddAsync(photoEntity);
            await _uow.SaveChangesAsync();

            return accessiblePath;
        }
    }
}