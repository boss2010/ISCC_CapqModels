using Microsoft.AspNetCore.Mvc;

namespace Capqwebsite.Controllers
{
    public class SampleCollectionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
