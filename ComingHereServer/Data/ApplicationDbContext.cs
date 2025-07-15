using ComingHereShared.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace ComingHereServer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventAttendee> EventAttendees { get; set; }
        public DbSet<EventPhoto> EventPhotos { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<EventOrganizer> EventOrganizers { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }
        public DbSet<OrganizerCategory> OrganizerCategories { get; set; }
        public DbSet<ParticipantCategory> ParticipantCategories { get; set; }
        public DbSet<EventDetails> EventDetails { get; set; }
        public DbSet<EventContactInfo> EventContactInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<EventAttendee>()
                .HasOne(e => e.Event)
                .WithMany(e => e.Attendees)
                .HasForeignKey(e => e.EventId);

            builder.Entity<EventAttendee>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);

            builder.Entity<EventPhoto>()
                .HasOne(p => p.Event)
                .WithMany(e => e.Photos)
                .HasForeignKey(p => p.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Event>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь: Event -> Organizer
            builder.Entity<Event>()
                .HasOne(e => e.Organizer)
                .WithMany(o => o.Events)
                .HasForeignKey(e => e.OrganizerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь: Participant -> Event
            builder.Entity<EventParticipant>()
                .HasOne(p => p.Event)
                .WithMany(e => e.Participants)
                .HasForeignKey(p => p.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь: Event -> EventDetails (1:1)
            builder.Entity<Event>()
                .HasOne(e => e.Details)
                .WithOne(d => d.Event)
                .HasForeignKey<EventDetails>(d => d.Id)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь: EventDetails -> EventContactInfo (1:1)
            builder.Entity<EventDetails>()
                .HasOne(d => d.ContactInfo)
                .WithOne(ci => ci.EventDetails)
                .HasForeignKey<EventDetails>(d => d.ContactInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            // JSON-конвертер для LocalizedString
            var options = new JsonSerializerOptions();

            var localizedStringConverter = new ValueConverter<LocalizedString, string>(
                v => JsonSerializer.Serialize(v.Values, options),
                v => new LocalizedString
                {
                    Values = JsonSerializer.Deserialize<Dictionary<string, string>>(v, options) ?? new Dictionary<string, string>()
                }
            );

            // Применяем к Event
            builder.Entity<Event>().Property(e => e.Name).HasConversion(localizedStringConverter).HasColumnType("jsonb");
            builder.Entity<Event>().Property(e => e.Description).HasConversion(localizedStringConverter).HasColumnType("jsonb");
            builder.Entity<Event>().Property(e => e.Location).HasConversion(localizedStringConverter).HasColumnType("jsonb");

            // Применяем к EventDetails
            builder.Entity<EventDetails>().Property(d => d.Address).HasConversion(localizedStringConverter).HasColumnType("jsonb");

            // Применяем к EventOrganizer
            builder.Entity<EventOrganizer>().Property(o => o.Name).HasConversion(localizedStringConverter).HasColumnType("jsonb");
            builder.Entity<EventOrganizer>().Property(o => o.Description).HasConversion(localizedStringConverter).HasColumnType("jsonb");
            builder.Entity<EventOrganizer>().Property(o => o.Address).HasConversion(localizedStringConverter).HasColumnType("jsonb");

            // Применяем к EventParticipant
            builder.Entity<EventParticipant>().Property(p => p.Name).HasConversion(localizedStringConverter).HasColumnType("jsonb");
            builder.Entity<EventParticipant>().Property(p => p.Role).HasConversion(localizedStringConverter).HasColumnType("jsonb");
        }
    }
}