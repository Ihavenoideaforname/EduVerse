using EduVerse.Data;
using EduVerse.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EduVerse.Controllers
{
    public class SchoolManagerController : Controller
    {
        private readonly EduVerseContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public SchoolManagerController(EduVerseContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
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
                Id = u.Id,
                Name = u.Name,
                Surname = u.Surname,
                Username = u.UserName!,
                SchoolRole = u.SchoolAccount!.SchoolRole!.Name
            }).ToList();

            ViewBag.Roles = _context.SchoolRoles.Where(sr => sr.SchoolId == SchoolId).Select(sr => sr.Name).ToList();
            ViewBag.SchoolId = SchoolId;

            return View(model);
        }
    }
}
