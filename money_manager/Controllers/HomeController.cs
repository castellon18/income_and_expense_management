using Microsoft.AspNetCore.Mvc;

namespace money_manager.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NotFoundPage()
        {
            return View();
        }
    }
}
