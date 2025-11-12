using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EduVerse.Models;

namespace EduVerse.Data
{
    public class EduVerseContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public EduVerseContext(DbContextOptions<EduVerseContext> options) : base(options)
        {
        }

        public override DbSet<User> Users { get; set; }
        public override DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // (User)N <-> N(Role)
            modelBuilder.Entity<UserRole>(ur =>
            {
                ur.HasKey(x => new { x.UserId, x.RoleId });

                ur.HasOne(x => x.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(x => x.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                ur.HasOne(x => x.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(x => x.RoleId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            foreach(var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var boolProperties = entityType.ClrType
                    .GetProperties()
                    .Where(p => p.PropertyType == typeof(bool));

                foreach(var property in boolProperties)
                {
                    modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<int>();
                }
            }
        }
    }
}
