using EduVerse.Data;
using EduVerse.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace EduVerse.Services
{
    public interface ISchoolService
    {
        Task CreateSchoolAsync(Guid RequestId);
    }

    public class SchoolService : ISchoolService
    {
        private readonly EduVerseContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public SchoolService(EduVerseContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task CreateSchoolAsync(Guid RequestId)
        {
            var Request = _context.SignUpRequests.FirstOrDefault(u => u.Id == RequestId);

            if(Request == null)
            {
                throw new Exception("User not found.");
            }

            var NewSchool = new School
            {
                SignUpRequestId = RequestId,
                Name = Request.SchoolName,
                NormalizedName = Request.NormalizedSchoolName,
                NameShortcut = Request.SchoolNameShortcut,
                NormalizedNameShortcut = Request.NormalizedSchoolNameShortcut,
                Email = Request.SchoolEmail,
                NormalizedEmail = Request.NormalizedSchoolEmail,
                PhoneNumber = Request.SchoolPhoneNumber,
                Street = Request.Street,
                BuildingNumber = Request.BuildingNumber,
                PostalCode = Request.PostalCode,
                City = Request.City,
                State = Request.State,
                Country = Request.Country
            };

            _context.Schools.Add(NewSchool);

            var PrincipalRole = new SchoolRole
            {
                Id = Guid.NewGuid(),
                SchoolId = NewSchool.Id,
                Name = "Principal",
                NormalizedName = "PRINCIPAL",
                Hierarchy = 0,
                IsParent = false,
                IsStudent = false,
                IsStaff = true,
                CanManageAccounts = true,
                CanManageRoles = true,
                CanManageGroups = true,
                CanManageCourses = true,
                CanManageStudents = true
            };

            _context.SchoolRoles.Add(PrincipalRole);

            var PrincipalUser = new User
            {
                Id = new Guid(),
                Name = Request.PrincipalName,
                Surname = Request.PrincipalSurname,
                UserName = Request.PrincipalName.ToLower() + "." + Request.PrincipalSurname.ToLower(),
                NormalizedUserName = Request.PrincipalName.ToUpper() + "." + Request.PrincipalSurname.ToUpper(),
                Email = Request.PrincipalName.ToLower() + "." + Request.PrincipalSurname.ToLower() + "@eduverse." + Request.SchoolNameShortcut.ToLower() + ".edu.com",
                NormalizedEmail = Request.PrincipalName.ToUpper() + "." + Request.PrincipalSurname.ToUpper() + "@EDUVERSE." + Request.SchoolNameShortcut.ToUpper() + ".EDU.COM",
            };

            PrincipalUser.PasswordHash = _passwordHasher.HashPassword(PrincipalUser, Request.PrincipalName.ToLower() + "." + Request.PrincipalSurname.ToLower() + "123");

            _context.Users.Add(PrincipalUser);

            Role UserRole = _context.Roles.First(r => r.NormalizedName == "USER");
            _context.UserRoles.Add(new UserRole { UserId = PrincipalUser.Id, RoleId = UserRole.Id });

            var PrincipalAccount = new SchoolAccount
            {
                Id = Guid.NewGuid(),
                UserId = PrincipalUser.Id,
                SchoolId = NewSchool.Id,
                SchoolRoleId = PrincipalRole.Id
            };

            _context.SchoolAccounts.Add(PrincipalAccount);
            PrincipalUser.SchoolAccountId = PrincipalAccount.Id;

            await _context.SaveChangesAsync();
        }
    }
}
