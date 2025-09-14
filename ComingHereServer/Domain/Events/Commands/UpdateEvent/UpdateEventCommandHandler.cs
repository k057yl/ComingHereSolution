using ComingHereServer.Data.Interfaces;
using ComingHereShared.Entities;
using MediatR;

namespace ComingHereServer.Domain.Events.Commands.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, bool>
    {
        private readonly IUnitOfWork _uow;

        public UpdateEventCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var ev = await _uow.Events.GetByIdWithDetailsAsync(request.EventId);
            if (ev == null)
                return false;

            var dto = request.Dto;

            // Обновляем основные поля
            ev.Name = new LocalizedString(dto.Name);
            ev.Description = new LocalizedString(dto.Description);
            ev.Location = new LocalizedString(dto.Location);
            ev.CategoryId = dto.CategoryId;
            ev.OrganizerId = dto.OrganizerId;
            ev.IsVip = dto.IsVip;
            ev.IsRecurring = dto.IsRecurring;

            // Детали события
            ev.Details.Address = dto.Details.Address;
            ev.Details.Price = dto.Details.Price;
            ev.Details.MaxAttendees = dto.Details.MaxAttendees;
            ev.Details.Latitude = dto.Details.Latitude;
            ev.Details.Longitude = dto.Details.Longitude;
            ev.Details.ContactInfo.Phone = dto.Details.ContactInfo.Phone;
            ev.Details.ContactInfo.Email = dto.Details.ContactInfo.Email;
            ev.Details.ContactInfo.Website = dto.Details.ContactInfo.Website;
            ev.Details.ContactInfo.Telegram = dto.Details.ContactInfo.Telegram;
            ev.Details.ContactInfo.Instagram = dto.Details.ContactInfo.Instagram;

            if (!dto.IsRecurring)
            {
                ev.StartTime = dto.StartTime;
                ev.EndTime = dto.EndTime;
            }
            else
            {
                // Удаляем старые расписания
                var existingSchedules = await _uow.EventSchedules.FindAsync(s => s.EventId == ev.Id);
                foreach (var s in existingSchedules)
                {
                    _uow.EventSchedules.Remove(s);
                }

                // Добавляем новые
                if (dto.Schedules != null)
                {
                    foreach (var s in dto.Schedules)
                    {
                        await _uow.EventSchedules.AddAsync(new EventSchedule
                        {
                            EventId = ev.Id,
                            DayOfWeek = s.DayOfWeek,
                            StartTime = s.StartTime,
                            EndTime = s.EndTime,
                            StartDate = s.StartDate,
                            EndDate = s.EndDate
                        });
                    }
                }
            }

            // Обновляем участников
            if (dto.ParticipantIds != null)
            {
                var existingParticipants = await _uow.EventParticipants.FindAsync(p => p.EventId == ev.Id);
                foreach (var p in existingParticipants)
                    p.EventId = 0; // отвязка

                var participants = await _uow.EventParticipants.FindAsync(p => dto.ParticipantIds.Contains(p.Id));
                foreach (var p in participants)
                    p.EventId = ev.Id;
            }

            await _uow.SaveChangesAsync();
            return true;
        }
    }
}