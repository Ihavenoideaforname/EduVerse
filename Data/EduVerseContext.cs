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
        public DbSet<SignUpRequest> SignUpRequests { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<SchoolRole> SchoolRoles { get; set; }
        public DbSet<SchoolAccount> SchoolAccounts { get; set; }
        public DbSet<StudentParents> StudentParents { get; set; }
        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // N (User) <-> (Role) N
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

            // 1 (School) <-> (SignUpRequest) 1
            modelBuilder.Entity<School>()
                .HasOne(s => s.SignUpRequest)
                .WithOne()
                .HasForeignKey<School>(s => s.SignUpRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1 (SchoolAccount[Teacher]) <-> (Group) 1
            modelBuilder.Entity<Group>()
                .HasOne(g => g.Teacher)
                .WithOne(s => s.SupervisedGroup)
                .HasForeignKey<Group>(g => g.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            // N (SchoolAccount[Students]) <-> (Group) 1
            modelBuilder.Entity<Group>()
                .HasMany(g => g.Students)
                .WithOne(s => s.Group)
                .HasForeignKey(s => s.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            // 1 (School) <-> (SchoolRoles) N
            modelBuilder.Entity<School>()
                .HasMany(s => s.SchoolRoles)
                .WithOne(sr => sr.School)
                .HasForeignKey(sr => sr.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1 (School) <-> (SchoolAccounts) N
            modelBuilder.Entity<School>()
                .HasMany(s => s.SchoolAccounts)
                .WithOne(sa => sa.School)
                .HasForeignKey(sa => sa.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            // N (SchoolAccounts[Students]) <-> (SchoolAccounts[Parents]) N
            modelBuilder.Entity<StudentParents>()
                .HasKey(sp => new { sp.StudentId, sp.ParentId });

            modelBuilder.Entity<StudentParents>()
                .HasOne(sp => sp.Student)
                .WithMany(s => s.Parents)
                .HasForeignKey(sp => sp.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StudentParents>()
                .HasOne(sp => sp.Parent)
                .WithMany(p => p.Students)
                .HasForeignKey(sp => sp.ParentId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1 (SchoolRole) <-> (SchoolAccount) N
            modelBuilder.Entity<SchoolRole>()
                .HasMany(sr => sr.SchoolAccounts)
                .WithOne(sa => sa.SchoolRole)
                .HasForeignKey(sa => sa.SchoolRoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // 1 (User) <-> (SchoolAccount) 1
            modelBuilder.Entity<SchoolAccount>()
                .HasOne(sa => sa.User)
                .WithOne(u => u.SchoolAccount)
                .HasForeignKey<SchoolAccount>(sa => sa.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Status Enum Translation
            modelBuilder.Entity<SignUpRequest>()
                .Property(sur => sur.Status)
                .HasConversion<string>();

            // Bool to Int conversion
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
