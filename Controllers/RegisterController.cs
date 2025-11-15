using Microsoft.AspNetCore.Mvc;
using EduVerse.Data;
using EduVerse.Models;
using Microsoft.EntityFrameworkCore;
using EduVerse.Enums;
using System.Security.Claims;

namespace EduVerse.Controllers
{
    public class RegisterController : Controller
    {
        private readonly EduVerseContext _context;

        public RegisterController(EduVerseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult SendRequest() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendRequest(SignUpRequestViewModel model)
        {
            if(ModelState.IsValid) 
            {
                int ErrorCount = 0;

                if(_context.SignUpRequests.Where(sur => sur.NormalizedSchoolName == model.SchoolName.ToUpper()).Count() > 0)
                {
                    ModelState.AddModelError("SchoolName", "School is already registered.");
                    ErrorCount++;
                }

                if(_context.SignUpRequests.Where(sur => sur.NormalizedSchoolNameShortcut == model.SchoolNameShortcut.ToUpper()).Count() > 0)
                {
                    ModelState.AddModelError("SchoolNameShortcut", "School name shortcut is already used.");
                    ErrorCount++;
                }

                if(_context.SignUpRequests.Where(sur => sur.NormalizedSchoolEmail == model.SchoolEmail.ToLower()).Count() > 0)
                {
                    ModelState.AddModelError("SchoolEmail", "School is already registered.");
                    ErrorCount++;
                }

                if(ErrorCount > 0) 
                {
                    return View(model);
                }

                SignUpRequest NewRequest = new SignUpRequest
                {
                    SchoolName = model.SchoolName,
                    NormalizedSchoolName = model.SchoolName.ToUpper(),
                    SchoolNameShortcut = model.SchoolNameShortcut,
                    NormalizedSchoolNameShortcut = model.SchoolNameShortcut.ToUpper(),
                    SchoolEmail = model.SchoolEmail,
                    NormalizedSchoolEmail = model.SchoolEmail.ToLower(),
                    SchoolPhoneNumber = model.SchoolPhoneNumber,
                    Street = model.Street,
                    BuildingNumber = model.BuildingNumber,
                    PostalCode = model.PostalCode,
                    City = model.City,
                    State = model.State,
                    Country = model.Country,
                    PrincipalName = model.PrincipalName,
                    PrincipalSurname = model.PrincipalSurname,
                    RequestLetter = model.RequestLetter
                };

                _context.SignUpRequests.Add(NewRequest);
                await _context.SaveChangesAsync();

                TempData["WelcomeMessage"] = "Sign up request sent successfully! Please await a response..";
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult RequestList(string? Search, string? Status, string Sort = "date_desc")
        {
            if(!User.IsInRole("ADMIN"))
            {
                return NotFound();
            }

            var RequestEntities = _context.SignUpRequests.ToList();
            var Query = RequestEntities.Select(sur => new RequestListViewModel
            {
                Id = sur.Id,
                SchoolName = sur.SchoolName,
                PrincipalName = sur.PrincipalName + " " + sur.PrincipalSurname,
                RequestDate = sur.RequestedAt,
                RequestStatus = sur.Status.ToString()
            }).AsQueryable();

            if(!string.IsNullOrWhiteSpace(Search))
            {
                Query = Query.Where(q =>
                    q.SchoolName.ToLower().Contains(Search.ToLower()) ||
                    q.PrincipalName.ToLower().Contains(Search.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(Status))
            {
                Query = Query.Where(q => q.RequestStatus == Status);
            }

            Query = Sort switch
            {
                "date_asc" => Query.OrderBy(q => q.RequestDate),
                _ => Query.OrderByDescending(q => q.RequestDate)
            };

            var model = Query.ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult RequestDetails(Guid? id)
        {
            var Request = _context.SignUpRequests.Find(id);

            if(Request == null || !User.IsInRole("ADMIN"))
            {
                return NotFound();
            }

            RequestDetailsViewModel model = new RequestDetailsViewModel
            {
                Id = Request.Id,
                SchoolName = Request.SchoolName,
                SchoolNameShortcut = Request.SchoolNameShortcut,
                SchoolEmail = Request.SchoolEmail,
                SchoolPhoneNumber = Request.SchoolPhoneNumber,

                Street = Request.Street,
                BuildingNumber = Request.BuildingNumber,
                PostalCode = Request.PostalCode,
                City = Request.City,
                State = Request.State,
                Country = Request.Country,

                PrincipalName = Request.PrincipalName,
                PrincipalSurname = Request.PrincipalSurname,

                RequestLetter = Request.RequestLetter,
                RequestedAt = Request.RequestedAt,
                Status = Request.Status.ToString(),

                ReviewedBy = Request.ReviewedBy,
                ReviewedAt = Request.ReviewedAt
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(Guid id)
        {
            var Request = await _context.SignUpRequests.FindAsync(id);

            if(Request == null || !User.IsInRole("ADMIN"))
            {
                return NotFound();
            }

            var UserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string? AdminName = null;

            if(Guid.TryParse(UserIdClaim, out var UserId))
            {
                AdminName = await _context.Users.Where(u => u.Id == UserId).Select(u => u.Name + " " + u.Surname).FirstOrDefaultAsync();
            }

            if(AdminName == null) 
            {
                return NotFound();
            }

            Request.Status = RequestStatus.Approved;
            Request.ReviewedAt = DateTime.UtcNow;
            Request.ReviewedBy = AdminName;

            await _context.SaveChangesAsync();
            return RedirectToAction("RequestDetails", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(Guid id)
        {
            var Request = await _context.SignUpRequests.FindAsync(id);

            if(Request == null || !User.IsInRole("ADMIN"))
            {
                return NotFound();
            }

            var UserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string? AdminName = null;

            if(Guid.TryParse(UserIdClaim, out var UserId))
            {
                AdminName = await _context.Users.Where(u => u.Id == UserId).Select(u => u.Name + " " + u.Surname).FirstOrDefaultAsync();
            }

            if(AdminName == null)
            {
                return NotFound();
            }

            Request.Status = RequestStatus.Rejected;
            Request.ReviewedAt = DateTime.UtcNow;
            Request.ReviewedBy = AdminName;

            await _context.SaveChangesAsync();
            return RedirectToAction("RequestDetails", new { id });
        }
    }
}
