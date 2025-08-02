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

        public static void Sanitize(EventCreateDto dto, IHtmlSanitizingService sanitizer)
        {
            Sanitize(dto.Name, sanitizer);
            Sanitize(dto.Description, sanitizer);
            Sanitize(dto.Location, sanitizer);

            if (dto.Details != null)
            {
                Sanitize(dto.Details.Address, sanitizer);

                var contact = dto.Details.ContactInfo;
                contact.Phone = sanitizer.Sanitize(contact.Phone);
                contact.Email = sanitizer.Sanitize(contact.Email);
                contact.Website = sanitizer.Sanitize(contact.Website);
                contact.Telegram = sanitizer.Sanitize(contact.Telegram);
                contact.Instagram = sanitizer.Sanitize(contact.Instagram);
            }
        }
    }
}