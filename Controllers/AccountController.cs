using Microsoft.AspNetCore.Mvc;

namespace EduVerse.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
