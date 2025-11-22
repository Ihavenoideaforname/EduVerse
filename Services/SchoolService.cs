using EduVerse.Data;
using EduVerse.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Principal;

namespace EduVerse.Services
{
    public interface ISchoolService
    {
        Task CreateSchoolAsync(Guid RequestId);
        Task AddNewAccountAsync(NewSchoolAccountViewModel Model);
        Task EditAccountAsync(EditSchoolAccountViewModel Model);
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

            _context.SchoolRoles.AddRange(
                new SchoolRole { 
                    SchoolId = NewSchool.Id,
                    Name = "Teacher",
                    NormalizedName = "TEACHER",
                    Hierarchy = 4,
                    IsParent = false,
                    IsStudent = false,
                    IsStaff = true,
                    CanManageAccounts = false,
                    CanManageRoles = false,
                    CanManageGroups = true,
                    CanManageCourses = true,
                    CanManageStudents = true
                },
                
                new SchoolRole {
                    SchoolId = NewSchool.Id,
                    Name = "Student",
                    NormalizedName = "STUDENT",
                    Hierarchy = 8,
                    IsParent = false,
                    IsStudent = true,
                    IsStaff = false,
                    CanManageAccounts = false,
                    CanManageRoles = false,
                    CanManageGroups = false,
                    CanManageCourses = false,
                    CanManageStudents = false
                },

                new SchoolRole {
                    SchoolId = NewSchool.Id,
                    Name = "Parent",
                    NormalizedName = "PARENT",
                    Hierarchy = 10,
                    IsParent = true,
                    IsStudent = false,
                    IsStaff = false,
                    CanManageAccounts = false,
                    CanManageRoles = false,
                    CanManageGroups = false,
                    CanManageCourses = false,
                    CanManageStudents = false
                }
            );

            string UsernameSchema = char.ToLower(Request.PrincipalName[0]) + "." + Request.PrincipalSurname.ToLower() + "0." + Request.SchoolNameShortcut.ToLower() + "." + PrincipalRole.Name.ToLower();
            string EmailSchema = char.ToLower(Request.PrincipalName[0]) + "." + Request.PrincipalSurname.ToLower() + "0@eduverse." + Request.SchoolNameShortcut.ToLower() + "." + PrincipalRole.Name.ToLower() + ".edu.com";

            var PrincipalUser = new User
            {
                Id = new Guid(),
                Name = Request.PrincipalName,
                Surname = Request.PrincipalSurname,
                UserName = UsernameSchema,
                NormalizedUserName = UsernameSchema.ToUpper(),
                Email = EmailSchema,
                NormalizedEmail = EmailSchema.ToUpper(),
            };

            PrincipalUser.PasswordHash = _passwordHasher.HashPassword(PrincipalUser, Request.PrincipalName.ToLower() + "." + Request.PrincipalSurname.ToLower());

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

        public async Task AddNewAccountAsync(NewSchoolAccountViewModel Model)
        {
            string Shortcut = _context.Schools
                .Where(s => s.Id == Model.SchoolId)
                .Select(s => s.NameShortcut)
                .First();

            string RoleName = _context.SchoolRoles
                .Where(sr => sr.Id == Model.SchoolRoleId)
                .Select(sr => sr.Name)
                .First();

            string StartUsername =
                $"{char.ToLower(Model.Name[0])}." +
                $"{Model.Surname.ToLower()}";

            string EndUsername =
                $".{Shortcut.ToLower()}." +
                $"{RoleName.ToLower()}";

            var ExistingUsernames = _context.SchoolAccounts
                .Include(sa => sa.User)
                .Where(sa => sa.SchoolId == Model.SchoolId && (sa.User!.UserName!.StartsWith(StartUsername) && sa.User!.UserName!.EndsWith(EndUsername)))
                .Select(sa => sa.User!.UserName)
                .ToList();

            int Index = 0;

            while(ExistingUsernames.Contains(StartUsername + Index + EndUsername))
            {
                Index++;
            }

            string FinalUsername = StartUsername + Index + EndUsername;

            Model.Username = FinalUsername;
            Model.Email = $"{char.ToLower(Model.Name[0])}.{Model.Surname.ToLower()}{Index}" + $"@eduverse.{Shortcut.ToLower()}.{RoleName.ToLower()}.edu.com";

            var SchoolUser = new User
            {
                Id = new Guid(),
                Name = Model.Name,
                Surname = Model.Surname,
                UserName = Model.Username,
                NormalizedUserName = Model.Username.ToUpper(),
                Email = Model.Email,
                NormalizedEmail = Model.Email.ToUpper()
            };

            SchoolUser.PasswordHash = _passwordHasher.HashPassword(SchoolUser, Model.Password);

            _context.Users.Add(SchoolUser);

            Role UserRole = _context.Roles.First(r => r.NormalizedName == "USER");
            _context.UserRoles.Add(new UserRole { UserId = SchoolUser.Id, RoleId = UserRole.Id });

            var SchoolAccount = new SchoolAccount
            {
                Id = Guid.NewGuid(),
                UserId = SchoolUser.Id,
                SchoolId = Model.SchoolId,
                SchoolRoleId = Model.SchoolRoleId
            };

            _context.SchoolAccounts.Add(SchoolAccount);
            SchoolUser.SchoolAccountId = SchoolAccount.Id;

            await _context.SaveChangesAsync();
        }

        public async Task EditAccountAsync(EditSchoolAccountViewModel Model)
        {
            var user = _context.SchoolAccounts.Where(sa => sa.Id == Model.SchoolAccountId).Include(sa => sa.User).Include(sa => sa.SchoolRole).FirstOrDefault();
            if(user == null)
            {
                return;
            }

            bool NeedNewNames = !string.Equals(user.User!.Name, Model.Name, StringComparison.Ordinal) ||
                !string.Equals(user.User!.Surname, Model.Surname, StringComparison.Ordinal) ||
                user.SchoolRoleId != Model.SchoolRoleId;

            if(NeedNewNames)
            {
                string Shortcut = _context.Schools
                    .Where(s => s.Id == Model.SchoolId)
                    .Select(s => s.NameShortcut)
                    .First();

                string RoleName = _context.SchoolRoles
                    .Where(sr => sr.Id == Model.SchoolRoleId)
                    .Select(sr => sr.Name)
                    .First();

                string StartUsername =
                    $"{char.ToLower(Model.Name[0])}." +
                    $"{Model.Surname.ToLower()}";

                string EndUsername =
                    $".{Shortcut.ToLower()}." +
                    $"{RoleName.ToLower()}";

                var ExistingUsernames = _context.SchoolAccounts
                    .Include(sa => sa.User)
                    .Where(sa => sa.SchoolId == Model.SchoolId && sa.Id != Model.SchoolAccountId && (sa.User!.UserName!.StartsWith(StartUsername) && sa.User!.UserName!.EndsWith(EndUsername)))
                    .Select(sa => sa.User!.UserName)
                    .ToList();

                int Index = 0;

                while(ExistingUsernames.Contains(StartUsername + Index + EndUsername))
                {
                    Index++;
                }

                string FinalUsername = StartUsername + Index + EndUsername;

                Model.Username = FinalUsername;
                Model.Email = $"{char.ToLower(Model.Name[0])}.{Model.Surname.ToLower()}{Index}" + $"@eduverse.{Shortcut.ToLower()}.{RoleName.ToLower()}.edu.com";
            }

            user.User.Name = Model.Name;
            user.User.Surname = Model.Surname;
            user.User.UserName = Model.Username;
            user.User.NormalizedUserName = Model.Username.ToUpper();
            user.User.Email = Model.Email;
            user.User.NormalizedEmail = Model.Email.ToUpper();
            user.User.PasswordHash = _passwordHasher.HashPassword(user.User, Model.Password);

            _context.Users.Update(user.User);

            user.SchoolRoleId = Model.SchoolRoleId;

            _context.SchoolAccounts.Update(user);

            await _context.SaveChangesAsync();
        }
    }
}
