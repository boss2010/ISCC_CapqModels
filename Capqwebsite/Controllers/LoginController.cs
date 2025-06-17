using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Capqwebsite.Controllers
{
    public class LoginController : Controller
    {
        [AllowAnonymous]
        [Route("/Login/Index")]

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GoDataEntryMenu(string userName, string password)
        {
            if (userName == "admin" && password == "admin@123")
            {
                
                CookieOptions option = new CookieOptions();

                HttpContext.Session.SetString("UserSession", "Authenticated");
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}
