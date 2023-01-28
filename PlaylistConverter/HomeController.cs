using Microsoft.AspNetCore.Mvc;

namespace PlaylistConverter
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
