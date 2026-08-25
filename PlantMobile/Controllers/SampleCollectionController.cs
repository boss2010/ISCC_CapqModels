using Microsoft.AspNetCore.Mvc;

namespace PlantMobile.Controllers
{
    public class SampleCollectionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
