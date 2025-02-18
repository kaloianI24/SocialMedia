using Microsoft.AspNetCore.Mvc;

namespace SocialMedia.Areas.Administration.Controllers
{
    public class ReactionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
