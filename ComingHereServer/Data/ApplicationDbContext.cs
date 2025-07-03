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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Связи
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

            var options = new JsonSerializerOptions();

            var localizedStringConverter = new ValueConverter<LocalizedString, string>(
                v => JsonSerializer.Serialize(v.Values, options),
                v => new LocalizedString
                {
                    Values = JsonSerializer.Deserialize<Dictionary<string, string>>(v, options) ?? new Dictionary<string, string>()
                }
            );

            builder.Entity<Event>().Property(e => e.Name).HasConversion(localizedStringConverter).HasColumnType("jsonb");
            builder.Entity<Event>().Property(e => e.Description).HasConversion(localizedStringConverter).HasColumnType("jsonb");
            builder.Entity<Event>().Property(e => e.Location).HasConversion(localizedStringConverter).HasColumnType("jsonb");
            builder.Entity<Event>().Property(e => e.OrganizerDisplayName).HasConversion(localizedStringConverter).HasColumnType("jsonb");
        }
    }
}