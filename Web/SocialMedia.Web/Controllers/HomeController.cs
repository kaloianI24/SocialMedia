using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Areas.Identity.Data;
using SocialMedia.Models;
using SocialMedia.Service.Mappings;
using SocialMedia.Web.Models.Post;
using System.Threading.Tasks;
using System;

namespace SocialMedia.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<SocialMediaUser> _userManager;
        private readonly IEmailSender _emailSender;

        public HomeController(ILogger<HomeController> logger, UserManager<SocialMediaUser> userManager, IEmailSender emailSender)
        {
            _logger = logger;
            _userManager = userManager;
            _emailSender = emailSender;
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
                .ThenInclude(p => p.Tags)
            .Include(u => u.TaggedPosts)
                .ThenInclude(p => p.Attachments)
            .Include(u => u.TaggedPosts)
                .ThenInclude(p => p.TaggedUsers)
            .Include(u => u.Followers)
            .Include(u => u.Friends)
            .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
        }

        public async Task<IActionResult> Register()
        {
            var user = new SocialMediaUser { UserName = "newUser", Email = "user@example.com" }; // Assume registration happened here
           
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = Url.Action("ConfirmEmail", "Home", new { userId = user.Id, token = token }, protocol: Request.Scheme);

           
            string subject = "Email Confirmation";
            string message = $"Please confirm your email by clicking the link below: <a href='{confirmationLink}'>Confirm Email</a>";

            await _emailSender.SendEmailAsync(user.Email, subject, message);

            return View();
        }

       
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
