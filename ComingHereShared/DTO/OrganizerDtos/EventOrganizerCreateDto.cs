﻿using ComingHereShared.Entities;

namespace ComingHereShared.DTO.OrganizerDtos
{
    public class EventOrganizerCreateDto
    {
        public LocalizedString Name { get; set; } = new();
        public LocalizedString Description { get; set; } = new();
        public LocalizedString Address { get; set; } = new();

        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? Telegram { get; set; }
        public string? Instagram { get; set; }

        public string? ApplicationUserId { get; set; }
        public int? CategoryId { get; set; }
    }
}
