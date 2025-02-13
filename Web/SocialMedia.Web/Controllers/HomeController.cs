using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Areas.Identity.Data;
using SocialMedia.Models;
using SocialMedia.Service.Mappings;
using SocialMedia.Web.Models.Post;
using static SocialMedia.Service.Mappings.SocialMediaPostMappings;

namespace SocialMedia.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<SocialMediaUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<SocialMediaUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await GetUser();

            ViewData["ProfilePictureUrl"] = user?.ProfilePicture?.CloudUrl;

            return View(user);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private Task<SocialMediaUser> GetUser()
        {
            return _userManager.Users
            .Include(u => u.ProfilePicture)
            .Include(u => u.Posts)
                .ThenInclude(p => p.Attachments)
             .Include(u => u.Posts)
                .ThenInclude(p=> p.Tags)
            .Include(u => u.TaggedPosts)
                .ThenInclude(p => p.Attachments)
            .Include(u => u.TaggedPosts)
                .ThenInclude(p => p.TaggedUsers)
            .Include(u => u.Followers)
            .Include(u => u.Friends)
            .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
        }
    }
}
