using EduVerse.Models;
using EduVerse.Data;
using Microsoft.AspNetCore.Identity;

namespace EduVerse.Services
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<EduVerseContext>();
            var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

            await context.Database.EnsureCreatedAsync();

            await SeedRolesAsync(context);
            await SeedAdminUserAsync(context, hasher);
        }

        private static Task SeedRolesAsync(EduVerseContext context)
        {
            if(context.Roles.Count() == 0)
            {
                context.Roles.AddRange(
                    new Role { Id = Guid.NewGuid(), Name = "Admin", NormalizedName = "ADMIN" },
                    new Role { Id = Guid.NewGuid(), Name = "User", NormalizedName = "USER" }
                );

                context.SaveChanges();
            }

            return Task.CompletedTask;
        }

        private static Task SeedAdminUserAsync(EduVerseContext context, IPasswordHasher<User> hasher)
        {
            if(context.Users.Count(u => u.UserName == "Admin0") == 0)
            {
                User admin = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "Admin0",
                    NormalizedUserName = "ADMIN0",
                    Name = "ADMIN",
                    Surname = "ADMIN",
                    Email = "admin0@eduverse.com",
                    NormalizedEmail = "ADMIN0@EDUVERSE.COM",
                    PhoneNumber = "000-000-000"
                };

                admin.PasswordHash = hasher.HashPassword(admin, "Admin0-Pass");

                context.Users.Add(admin);

                Role adminRole = context.Roles.First(r => r.NormalizedName == "ADMIN");
                context.UserRoles.Add(new UserRole { UserId = admin.Id, RoleId = adminRole.Id });

                context.SaveChanges();
            }

            return Task.CompletedTask;
        }
    }
}
