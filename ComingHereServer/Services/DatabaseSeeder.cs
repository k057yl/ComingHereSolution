using ComingHereServer.Data;
using ComingHereShared.Constants;
using ComingHereShared.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ComingHereServer.Services
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            await SeedRolesAsync(serviceProvider);
            await SeedAdminAsync(serviceProvider);
            await SeedCategoriesAsync(serviceProvider);
        }

        private static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = new[] { Roles.Gala, Roles.User };
            foreach (var role in roles)
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
        }

        private static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var config = serviceProvider.GetRequiredService<IConfiguration>();

            var email = config["AdminUser:Email"];
            var password = config["AdminUser:Password"];
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) return;

            var admin = await userManager.FindByEmailAsync(email);
            if (admin == null)
            {
                admin = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true };
                var result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, Roles.Gala);
            }
            else
            {
                if (!await userManager.IsInRoleAsync(admin, Roles.Gala))
                    await userManager.AddToRoleAsync(admin, Roles.Gala);
            }
        }

        private static async Task SeedCategoriesAsync(IServiceProvider serviceProvider)
        {
            var db = serviceProvider.GetRequiredService<ApplicationDbContext>();
            string[] categories = { "Art", "Music", "Science", "Workshop", "Theatre", "Gaming", "Education" };

            foreach (var c in categories)
                if (!await db.Set<EventCategory>().AnyAsync(x => x.Name == c))
                    db.Set<EventCategory>().Add(new EventCategory { Name = c });

            await db.SaveChangesAsync();
        }
    }
}
