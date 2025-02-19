using Microsoft.AspNetCore.Mvc;
using SocialMedia.Web.Models.Reaction;

namespace SocialMedia.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class ReactionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult CreateConfirm(CreateReactionModel model)
        //{

        //}
    }
}
