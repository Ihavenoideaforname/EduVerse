using EduVerse.Data;
using EduVerse.Models;
using EduVerse.Services;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace EduVerse.Controllers
{
    public class SchoolManagerController : Controller
    {
        private readonly EduVerseContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly ISchoolService _schoolService;

        public SchoolManagerController(EduVerseContext context, ISchoolService schoolService)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
            _schoolService = schoolService;
        }

        [HttpGet]
        public IActionResult ManageAccounts(Guid SchoolId, string? Search, string? Role)
        {
            if(!User.IsInRole("USER"))
            {
                return NotFound();
            }

            var UserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            User? UserData = null;

            if(Guid.TryParse(UserIdClaim, out var UserId))
            {
                UserData = _context.Users.Where(u => u.Id == UserId).Include(u => u.SchoolAccount).FirstOrDefault();
            }
            if(UserData == null) 
            {
                return NotFound();
            }

            bool CanManageAccounts = HttpContext.Session.GetString("CanManageAccounts") == "True";

            if(!CanManageAccounts || UserData.SchoolAccount!.SchoolId != SchoolId) 
            {
                return Unauthorized();
            }

            var Query = _context.Users
                .Include(u => u.SchoolAccount)
                .ThenInclude(sa => sa!.SchoolRole)
                .Where(u => u.SchoolAccount!.SchoolId == SchoolId)
                .AsQueryable();

            if(!string.IsNullOrWhiteSpace(Search))
            {
                Query = Query.Where(u =>
                    u.Name.ToLower().Contains(Search.ToLower()) ||
                    u.Surname.ToLower().Contains(Search.ToLower()) ||
                    u.UserName!.ToLower().Contains(Search.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(Role))
            {
                var RoleId = _context.SchoolRoles.FirstOrDefault(sr => sr.Name == Role && sr.SchoolId == SchoolId)!.Id;
                Query = Query.Where(u => u.SchoolAccount!.SchoolRoleId! == RoleId);
            }

            List<UserListViewModel> model = Query.Select(u => new UserListViewModel
            {
                Id = u.SchoolAccount!.Id,
                Name = u.Name,
                Surname = u.Surname,
                Username = u.UserName!,
                SchoolRole = u.SchoolAccount!.SchoolRole!.Name
            }).ToList();

            ViewBag.Roles = _context.SchoolRoles.Where(sr => sr.SchoolId == SchoolId).Select(sr => sr.Name).ToList();
            ViewBag.SchoolId = SchoolId;

            return View(model);
        }

        [HttpGet]
        public IActionResult AddNewSchoolAccount(Guid SchoolId)
        {
            if(!User.IsInRole("USER"))
            {
                return NotFound();
            }

            var UserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            User? UserData = null;

            if(Guid.TryParse(UserIdClaim, out var UserId))
            {
                UserData = _context.Users.Where(u => u.Id == UserId).Include(u => u.SchoolAccount).FirstOrDefault();
            }
            if(UserData == null)
            {
                return NotFound();
            }

            bool CanManageAccounts = HttpContext.Session.GetString("CanManageAccounts") == "True";

            if(!CanManageAccounts || UserData.SchoolAccount!.SchoolId != SchoolId)
            {
                return Unauthorized();
            }

            ViewBag.SchoolId = SchoolId;
            ViewBag.Shortcut = _context.Schools.Where(s => s.Id == SchoolId).Select(s => s.NameShortcut).FirstOrDefault();
            ViewBag.SchoolRoles = _context.SchoolRoles.Where(sr => sr.SchoolId ==  SchoolId).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewSchoolAccount(NewSchoolAccountViewModel model)
        {
            if(ModelState.IsValid)
            {
                if(model.SchoolRoleId == Guid.Empty)
                {
                    ModelState.AddModelError("SchoolRoleId", "Please select role for that account.");

                    ViewBag.SchoolId = model.SchoolId;
                    ViewBag.Shortcut = _context.Schools.Where(s => s.Id == model.SchoolId).Select(s => s.NameShortcut).FirstOrDefault();
                    ViewBag.SchoolRoles = _context.SchoolRoles.Where(sr => sr.SchoolId == model.SchoolId).ToList();

                    return View(model);
                }
                
                await _schoolService.AddNewAccountAsync(model);

                return RedirectToAction("ManageAccounts", new {SchoolId = model.SchoolId});
            }

            ViewBag.SchoolId = model.SchoolId;
            ViewBag.Shortcut = _context.Schools.Where(s => s.Id == model.SchoolId).Select(s => s.NameShortcut).FirstOrDefault();
            ViewBag.SchoolRoles = _context.SchoolRoles.Where(sr => sr.SchoolId == model.SchoolId).ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult SchoolAccountDetails(Guid SchoolId, Guid SchoolAccountId)
        {
            if(!User.IsInRole("USER"))
            {
                return NotFound();
            }

            var UserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            User? UserData = null;

            if(Guid.TryParse(UserIdClaim, out var UserId))
            {
                UserData = _context.Users.Where(u => u.Id == UserId).Include(u => u.SchoolAccount).FirstOrDefault();
            }

            if(UserData == null)
            {
                return NotFound();
            }

            bool CanManageAccounts = HttpContext.Session.GetString("CanManageAccounts") == "True";

            if(!CanManageAccounts || UserData.SchoolAccount!.SchoolId != SchoolId)
            {
                return Unauthorized();
            }

            var user = _context.SchoolAccounts.Where(sa => sa.SchoolId == SchoolId && sa.Id == SchoolAccountId)
                .Include(sa => sa.User)
                .Include(sa => sa.School)
                .Include(sa => sa.SchoolRole)
                .FirstOrDefault();

            SchoolAccountViewModel model = new SchoolAccountViewModel
            {
                Name = user!.User!.Name,
                Surname = user.User.Surname,
                Username = user.User.UserName!,
                Email = user.User.Email!,
                PhoneNumber = user.User.PhoneNumber == null ? "" : user.User.PhoneNumber,
                SchoolName = user.School!.Name,
                RoleName = user.SchoolRole!.Name,
                SchoolId = SchoolId,
                SchoolAccountId = SchoolAccountId
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult EditSchoolAccount(Guid SchoolId, Guid SchoolAccountId)
        {
            if(!User.IsInRole("USER"))
            {
                return NotFound();
            }

            var UserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            User? UserData = null;

            if(Guid.TryParse(UserIdClaim, out var UserId))
            {
                UserData = _context.Users.Where(u => u.Id == UserId).Include(u => u.SchoolAccount).FirstOrDefault();
            }

            if(UserData == null)
            {
                return NotFound();
            }

            bool CanManageAccounts = HttpContext.Session.GetString("CanManageAccounts") == "True";

            if(!CanManageAccounts || UserData.SchoolAccount!.SchoolId != SchoolId)
            {
                return Unauthorized();
            }

            var user = _context.SchoolAccounts.Where(sa => sa.Id == SchoolAccountId && sa.SchoolId == SchoolId)
                .Include(sa => sa.User)
                .Include(sa => sa.SchoolRole)
                .FirstOrDefault();

            EditSchoolAccountViewModel model = new EditSchoolAccountViewModel
            {
                Name = user!.User!.Name,
                Surname = user.User.Surname,
                Username = user.User.UserName!,
                Email = user.User.Email!,
                Password = user.User.Name.ToLower() + "." + user.User.Surname.ToLower(),
                SchoolAccountId = SchoolAccountId,
                SchoolId = SchoolId,
                SchoolRoleId = user.SchoolRoleId
            };

            ViewBag.CanEdit = true;

            if(!VerifyPassword(model.Name.ToLower() + "." + model.Surname.ToLower(), user.User))
            {
                ModelState.AddModelError("", "This account is presently active by its owner. Modifications to active accounts are not permitted.");
                ViewBag.CanEdit = false;
            }

            ViewBag.SchoolId = SchoolId;
            ViewBag.SchoolAccountId = SchoolAccountId;
            ViewBag.Shortcut = _context.Schools.Where(s => s.Id == SchoolId).Select(s => s.NameShortcut).FirstOrDefault();
            ViewBag.SchoolRoles = _context.SchoolRoles.Where(sr => sr.SchoolId == SchoolId).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSchoolAccount(EditSchoolAccountViewModel model)
        {
            if(ModelState.IsValid)
            {
                if(model.SchoolRoleId == Guid.Empty)
                {
                    ModelState.AddModelError("SchoolRoleId", "Please select role for that account.");

                    ViewBag.SchoolId = model.SchoolId;
                    ViewBag.Shortcut = _context.Schools.Where(s => s.Id == model.SchoolId).Select(s => s.NameShortcut).FirstOrDefault();
                    ViewBag.SchoolRoles = _context.SchoolRoles.Where(sr => sr.SchoolId == model.SchoolId).ToList();

                    return View(model);
                }

                await _schoolService.EditAccountAsync(model);

                return RedirectToAction("SchoolAccountDetails", new { SchoolId = model.SchoolId, SchoolAccountId = model.SchoolAccountId });
            }

            return View(model);
        }

        private bool VerifyPassword(string enteredPassword, User user)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, enteredPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
