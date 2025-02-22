using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Areas.Identity.Data;
using SocialMedia.Models;
using SocialMedia.Service.Friends;
using SocialMedia.Service.Mappings;
using SocialMedia.Web.Models.Post;
using System.Threading.Tasks;
using System;
using SocialMedia.Service.Reaction;



namespace SocialMedia.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<SocialMediaUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IFriendRequestService _friendRequestsService;
        private readonly IReactionService _reactionService;
        public HomeController(ILogger<HomeController> logger, UserManager<SocialMediaUser> userManager, IEmailSender emailSender, IFriendRequestService friendRequestsService, IReactionService reactionService)

        {
            _logger = logger;
            _userManager = userManager;
            _emailSender = emailSender;
            _friendRequestsService = friendRequestsService;
            _reactionService = reactionService;

        }

        public async Task<IActionResult> Index()
        {
            var user = await GetUser();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }


            var roles = await _userManager.GetRolesAsync(user);
            bool isAdmin = roles.Contains("Admin");

            this.ViewData["Reactions"] = this._reactionService.GetAll().ToList();

            ViewData["IsAdmin"] = isAdmin;
            ViewData["ProfilePictureUrl"] = user.ProfilePicture?.CloudUrl;
            ViewData["FriendRequests"] = user.ReceivedFriendRequests.ToList();

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
            .Include(u => u.Following)
            .Include(u => u.Followers)
            .Include(u => u.Friends)
            .Include(u => u.ReceivedFriendRequests)
                .ThenInclude(r => r.CreatedBy)
                    .ThenInclude(u => u.ProfilePicture)
            .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
        }

        private Task<SocialMediaUser> GetUserById(string id)
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
            .Include(u => u.Following)
            .Include(u => u.Followers)
            .Include(u => u.Friends)
            .Include(u => u.ReceivedFriendRequests)
                .ThenInclude(r => r.CreatedBy)
                    .ThenInclude(u => u.ProfilePicture)
            .FirstOrDefaultAsync(u => u.Id == id);
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

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> SentFriendRequest([FromQuery] string friendId)
        {
            try
            {
                var receiver = await GetUserById(friendId);
                var sender = await GetUser();
                await _friendRequestsService.CreateFriendRequest(receiver, sender);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> AcceptFriendRequest([FromQuery] string requestId)
        {
            await _friendRequestsService.AcceptFriendRequest(requestId);
            return NoContent();
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteFriendRequest([FromQuery] string requestId)
        {
            await _friendRequestsService.DeleteFriendRequest(requestId);
            return NoContent();
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> RemoveFriend([FromQuery] string friendId)
        {
            try
            {
                var currentUser = await GetUserById(friendId);
                var friend = await GetUser();
                await _friendRequestsService.RemoveFriend(currentUser, friend);
                return NoContent();
            }
            
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Follow([FromQuery] string followingId)
        {
            try
            {
                var following = await GetUserById(followingId);
                var currentUser = await GetUser();
                await _friendRequestsService.Follow(currentUser, following);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Unfollow([FromQuery] string unfollowingId)
        {
            try
            {
                var unfollowing = await GetUserById(unfollowingId);
                var currentUser = await GetUser();
                await _friendRequestsService.Unfollow(currentUser, unfollowing);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
