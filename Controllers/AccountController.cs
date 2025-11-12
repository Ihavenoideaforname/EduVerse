using Microsoft.AspNetCore.Mvc;
using EduVerse.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using EduVerse.Data;
using Microsoft.AspNetCore.Authentication;

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

                    var user = _context.Users.FirstOrDefault(c => c.Id == userId);

                    TempData["WelcomeMessage"] = $"Login Successful! Welcome back {user?.UserName ?? "User"}";
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = _context.Users.Where(u => u.UserName == model.UserIdentifier || u.Email == model.UserIdentifier).FirstOrDefault();

                if(user == null || !VerifyPassword(model.Password, user))
                {
                    ModelState.AddModelError("Password", "Invalid User Identifier or Password.");
                    return View(model);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Email, user.Email!)
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

                return RedirectToAction("Index", "Home");
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
