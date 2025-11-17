using Microsoft.AspNetCore.Mvc;
using EduVerse.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using EduVerse.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EduVerse.Controllers
{
    public class AccountController : Controller
    {
        private readonly EduVerseContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public AccountController(EduVerseContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if(User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if(Guid.TryParse(userIdClaim, out var userId))
                {
                    HttpContext.Session.SetString("UserId", userId.ToString());

                    var user = _context.Users.Where(u => u.Id == userId)
                        .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                        .Include(u => u.SchoolAccount)
                        .FirstOrDefault();

                    bool IsAdmin = user!.UserRoles.Count(ur => ur.Role!.NormalizedName == "ADMIN") > 0;

                    if(!IsAdmin && user.SchoolAccount != null)
                    {
                        var Role = _context.SchoolRoles.Where(sr => sr.Id == user.SchoolAccount.SchoolRoleId).FirstOrDefault();

                        if(Role != null)
                        {
                            HttpContext.Session.SetString("RoleId", Role.Id.ToString());
                            HttpContext.Session.SetString("SchoolId", user.SchoolAccount.SchoolId.ToString());
                            HttpContext.Session.SetString("IsStudent", Role.IsStudent.ToString());
                            HttpContext.Session.SetString("IsParent", Role.IsParent.ToString());
                            HttpContext.Session.SetString("IsStaff", Role.IsStaff.ToString());
                            HttpContext.Session.SetString("CanManageAccounts", Role.CanManageAccounts.ToString());
                            HttpContext.Session.SetString("CanManageRoles", Role.CanManageRoles.ToString());
                            HttpContext.Session.SetString("CanManageGroups", Role.CanManageGroups.ToString());
                            HttpContext.Session.SetString("CanManageCourses", Role.CanManageCourses.ToString());
                            HttpContext.Session.SetString("CanManageStudents", Role.CanManageStudents.ToString());
                        }
                    }

                    TempData["WelcomeMessage"] = $"Login Successful! Welcome back {user?.UserName ?? "User"}";
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = _context.Users
                    .Where(u => u.UserName == model.UserIdentifier || u.Email == model.UserIdentifier)
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .Include(u => u.SchoolAccount)
                    .FirstOrDefault();

                if(user == null || !VerifyPassword(model.Password, user))
                {
                    ModelState.AddModelError("Password", "Invalid User Identifier or Password.");
                    return View(model);
                }

                bool IsAdmin = user.UserRoles.Count(ur => ur.Role!.NormalizedName == "ADMIN") > 0;
                string MostImportantRole = IsAdmin ? "ADMIN" : "USER";

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.Role, MostImportantRole)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "RememberMeCookie");

                var RememberMeProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = model.RememberMe ? DateTime.UtcNow.AddDays(14) : DateTime.UtcNow.AddMinutes(30),
                    AllowRefresh = true
                };

                await HttpContext.SignInAsync("RememberMeCookie", new ClaimsPrincipal(claimsIdentity), RememberMeProperties);
                HttpContext.Session.SetString("UserId", user.Id.ToString());

                if(!IsAdmin && user.SchoolAccount != null)
                {
                    var Role = _context.SchoolRoles.Where(sr => sr.Id == user.SchoolAccount.SchoolRoleId).FirstOrDefault();

                    if(Role != null)
                    {
                        HttpContext.Session.SetString("RoleId", Role.Id.ToString());
                        HttpContext.Session.SetString("SchoolId", user.SchoolAccount.SchoolId.ToString());
                        HttpContext.Session.SetString("IsStudent", Role.IsStudent.ToString());
                        HttpContext.Session.SetString("IsParent", Role.IsParent.ToString());
                        HttpContext.Session.SetString("IsStaff", Role.IsStaff.ToString());
                        HttpContext.Session.SetString("CanManageAccounts", Role.CanManageAccounts.ToString());
                        HttpContext.Session.SetString("CanManageRoles", Role.CanManageRoles.ToString());
                        HttpContext.Session.SetString("CanManageGroups", Role.CanManageGroups.ToString());
                        HttpContext.Session.SetString("CanManageCourses", Role.CanManageCourses.ToString());
                        HttpContext.Session.SetString("CanManageStudents", Role.CanManageStudents.ToString());
                    }
                }

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync("RememberMeCookie");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        private bool VerifyPassword(string enteredPassword, User user)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, enteredPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
