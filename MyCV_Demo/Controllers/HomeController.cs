using Microsoft.AspNetCore.Mvc;

namespace MyCV_Demo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
