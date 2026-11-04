using Microsoft.AspNetCore.Mvc;

namespace PlantMobile.Controllers
{
    public class UnavailableController : Controller
    {
        // Friendly replacement for raw 404 pages: tells the user to ask the admin.
        [HttpGet]
        public IActionResult Index(string? code)
        {
            ViewData["Title"] = "هذه الشاشة غير متاحة";
            ViewBag.Code = string.IsNullOrWhiteSpace(code) ? "404" : code;
            return View();
        }
    }
}
