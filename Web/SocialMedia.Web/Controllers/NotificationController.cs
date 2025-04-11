using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Areas.Identity.Data;
using SocialMedia.Data.Repositories;
using SocialMedia.Web.Models.Notifications;
using System.Security.Claims;

namespace SocialMedia.Controllers
{
    public class NotificationController : Controller
    {
        private readonly NotificationRepository _notificationRepo;
        private readonly UserManager<SocialMediaUser> _userManager; 

        public NotificationController(NotificationRepository notificationRepo, UserManager<SocialMediaUser> userManager)
        {
            _notificationRepo = notificationRepo;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.Users.Include(u => u.ProfilePicture).FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
            var notifications = _notificationRepo.GetAllForUser(currentUser.Id);

            var viewModel = notifications.Select(n => new NotificationViewModel
            {
                Message = n.Message,
                CreatedAt = n.CreatedAt
            }).ToList();
            ViewData["ProfilePictureUrl"] = currentUser?.ProfilePicture?.CloudUrl;
            var role = await _userManager.GetRolesAsync(currentUser);
            ViewData["IsAdmin"] = role.Contains("Administrator");
            return View(viewModel);
        }
    }
}
