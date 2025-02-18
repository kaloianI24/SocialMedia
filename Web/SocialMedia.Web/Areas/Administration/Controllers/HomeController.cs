using Microsoft.AspNetCore.Mvc;

namespace SocialMedia.Areas.Administration.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
