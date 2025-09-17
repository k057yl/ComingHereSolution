using ComingHereServer.Data.Interfaces;
using ComingHereServer.Services.Interfaces;
using ComingHereShared.Entities;
using MediatR;

namespace ComingHereServer.Domain.Events.Commands.CreateEvent
{
    public class CreateEventCommandHandler
        : BaseHandler, IRequestHandler<CreateEventCommand, int>
    {
        public CreateEventCommandHandler(IUnitOfWork uow, IHtmlSanitizingService sanitizer)
            : base(uow, sanitizer) { }

        public async Task<int> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // Проверка существования организатора
            if (!await _uow.Events.OrganizerExists(dto.OrganizerId))
                throw new Exception("Указанный организатор не существует.");

            var newEvent = new Event
            {
                Name = new LocalizedString(request.Dto.Name),
                Description = new LocalizedString(request.Dto.Description),
                Location = new LocalizedString(request.Dto.Location),
                CategoryId = dto.CategoryId,
                OrganizerId = dto.OrganizerId,
                IsVip = dto.IsVip,
                Details = new EventDetails
                {
                    Address = dto.Details.Address,
                    Price = dto.Details.Price,
                    MaxAttendees = dto.Details.MaxAttendees,
                    Latitude = dto.Details.Latitude,
                    Longitude = dto.Details.Longitude,
                    ContactInfo = new EventContactInfo
                    {
                        Phone = dto.Details.ContactInfo.Phone,
                        Email = dto.Details.ContactInfo.Email,
                        Website = dto.Details.ContactInfo.Website,
                        Telegram = dto.Details.ContactInfo.Telegram,
                        Instagram = dto.Details.ContactInfo.Instagram
                    }
                },
                IsRecurring = dto.IsRecurring
            };

            if (!dto.IsRecurring)
            {
                newEvent.StartTime = dto.StartTime;
                newEvent.EndTime = dto.EndTime;
            }

            await _uow.Events.AddAsync(newEvent);
            await _uow.SaveChangesAsync();

            // Расписание
            if (dto.IsRecurring && dto.Schedules != null)
            {
                foreach (var s in dto.Schedules)
                {
                    await _uow.EventSchedules.AddAsync(new EventSchedule
                    {
                        EventId = newEvent.Id,
                        DayOfWeek = s.DayOfWeek,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        StartDate = s.StartDate,
                        EndDate = s.EndDate
                    });
                }
                await _uow.SaveChangesAsync();
            }

            // Участники
            if (dto.ParticipantIds != null)
            {
                var participants = await _uow.EventParticipants.FindAsync(p => dto.ParticipantIds.Contains(p.Id));
                foreach (var participant in participants)
                    participant.EventId = newEvent.Id;

                await _uow.SaveChangesAsync();
            }

            return newEvent.Id;
        }
    }
}