using ComingHereShared.DTO.EventDtos;
using ComingHereShared.Entities;
using ComingHereServer.Interfaces;

namespace ComingHereServer.Services
{
    public static class DtoSanitizer
    {
        public static void Sanitize(LocalizedString localized, IHtmlSanitizingService sanitizer)
        {
            if (localized == null) return;

            var keys = localized.Values.Keys.ToList();
            foreach (var key in keys)
            {
                localized.Values[key] = sanitizer.Sanitize(localized.Values[key]);
            }
        }

        public static string Sanitize(string? value, IHtmlSanitizingService sanitizer)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return sanitizer.Sanitize(value);
        }

        public static void Sanitize(EventCreateDto dto, IHtmlSanitizingService sanitizer)
        {
            Sanitize(dto.Name, sanitizer);
            Sanitize(dto.Description, sanitizer);
            Sanitize(dto.Location, sanitizer);

            if (dto.Details != null)
            {
                Sanitize(dto.Details.Address, sanitizer);

                var contact = dto.Details.ContactInfo;
                contact.Phone = Sanitize(contact.Phone, sanitizer);
                contact.Email = Sanitize(contact.Email, sanitizer);
                contact.Website = Sanitize(contact.Website, sanitizer);
                contact.Telegram = Sanitize(contact.Telegram, sanitizer);
                contact.Instagram = Sanitize(contact.Instagram, sanitizer);
            }
        }
    }
}