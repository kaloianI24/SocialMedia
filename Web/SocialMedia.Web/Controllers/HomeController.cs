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
using System.Linq;
using SocialMedia.Data;
using System.Collections.Generic;
using SocialMedia.Service.SocialMediaPost;
using SocialMedia.Data;

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
        private readonly ISocialMediaPostService _postService;
        private readonly SocialMediaDbContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            UserManager<SocialMediaUser> userManager,
            IEmailSender emailSender,
            IFriendRequestService friendRequestsService,
            IReactionService reactionService,
            ISocialMediaPostService postService,
            SocialMediaDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _emailSender = emailSender;
            _friendRequestsService = friendRequestsService;
            _reactionService = reactionService;
            _postService = postService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await GetUserFeed();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var roles = await _userManager.GetRolesAsync(user);
            bool isAdmin = roles.Contains("Administrator");

            this.ViewData["Reactions"] = this._reactionService.GetAll().ToList();

            ViewData["IsAdmin"] = isAdmin;
            ViewData["ProfilePictureUrl"] = user.ProfilePicture?.CloudUrl;
            ViewData["FriendRequests"] = user.ReceivedFriendRequests.ToList();
            ViewData["SavedPosts"] = user.SavedPosts.Select(p => p.Id).ToList();
            var friendPosts = user.Friends.SelectMany(f => f.Posts).Where(p => p.DeletedOn == null).OrderByDescending(p => p.CreatedOn).ToList();
            var followingPosts = user.Following.SelectMany(f => f.Posts).Where(p => p.DeletedOn == null && p.Visibility.Equals("all") || p.Visibility.Equals("followers")).OrderByDescending(p => p.CreatedOn).ToList();
            var posts = friendPosts.Concat(followingPosts).ToList();

            var postWebModel = posts.Select(p => new TaggedPostWebModel
            {
                Id = p.Id,
                Description = p.Description,
                AttachmentUrls = p.Attachments.Select(a => a.CloudUrl).ToList(),
                Tags = p.Tags.Select(t => t.Name).ToList(),
                UserName = p.CreatedBy.UserName,
                ProfilePictureUrl = p.CreatedBy.ProfilePicture?.CloudUrl,
                CreatedOn = p.CreatedOn,
                CreatedById = p.CreatedBy.Id,
                TaggedUsersId = p.TaggedUsers.Select(u => u.Id).ToList(),
                TaggedUsersUserNames = p.TaggedUsers.Select(u => u.UserName).ToList(),
            }).ToList();

            return View(postWebModel);
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
                .Include(u => u.BlockedUsers)
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
                .Include(u => u.BlockedUsers)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IActionResult> Register()
        {
            var user = new SocialMediaUser { UserName = "newUser", Email = "user@example.com" };

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
            _logger.LogInformation($"Controller: Accepting friend request with ID: {requestId}");
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

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Block([FromQuery] string blockingId)
        {
            try
            {
                var blocking = await GetUserById(blockingId);
                var currentUser = await GetUser();
                await _friendRequestsService.Block(currentUser, blocking);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Unblock([FromQuery] string unblockingId)
        {
            try
            {
                var unblocking = await GetUserById(unblockingId);
                var currentUser = await GetUser();
                await _friendRequestsService.Unblock(currentUser, unblocking);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<SocialMediaUser> GetUserFeed()
        {
            return await _userManager.Users
            .Include(u => u.ProfilePicture)
            .Include(u => u.Following)
                .ThenInclude(f => f.ProfilePicture)
             .Include(u => u.Following)
                .ThenInclude(f => f.Posts.Where(p => p.DeletedOn == null))
                    .ThenInclude(p => p.Attachments)
              .Include(u => u.Following)
                .ThenInclude(f => f.Posts.Where(p => p.DeletedOn == null))
                    .ThenInclude(p => p.Tags)
            .Include(u => u.Following)
                .ThenInclude(f => f.Posts.Where(p => p.DeletedOn == null))
                    .ThenInclude(p => p.TaggedUsers)
            .Include(u => u.Friends)
                .ThenInclude(f => f.ProfilePicture)
             .Include(u => u.Friends)
                .ThenInclude(f => f.Posts.Where(p => p.DeletedOn == null))
                    .ThenInclude(p => p.Attachments)
             .Include(u => u.Friends)
                .ThenInclude(f => f.Posts.Where(p => p.DeletedOn == null))
                    .ThenInclude(p => p.Tags)
              .Include(u => u.Friends)
                .ThenInclude(f => f.Posts.Where(p => p.DeletedOn == null))
                    .ThenInclude(p => p.TaggedUsers)
              .Include(u => u.ReceivedFriendRequests)
                .ThenInclude(r => r.CreatedBy)
                    .ThenInclude(u => u.ProfilePicture)
              .Include(u => u.SavedPosts)
            .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
        }


        public async Task<IActionResult> Search(string query)
        {
            var currentUser = await GetUser();

            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction("Index");
            }

            query = query.ToLower();

          
            var blockedUserIds = currentUser.BlockedUsers?.Select(bu => bu.Id).ToList() ?? new List<string>();

           
            var rawUsers = await _userManager.Users
                .Where(u => !u.IsDeleted && u.Id != currentUser.Id)
                .Include(u => u.ProfilePicture)
                .ToListAsync();

            var users = rawUsers
                .Where(u => !blockedUserIds.Contains(u.Id))
                .Where(u => (u.UserName != null && u.UserName.ToLower().Contains(query)) ||
                            (u.Email != null && u.Email.ToLower().Contains(query)))
                .ToList();

            var orderedUsers = users
                .OrderByDescending(u => u.UserName != null && u.UserName.ToLower() == query)
                .ThenBy(u => u.UserName)
                .ToList();

           
            var allPosts = await _context.Posts
                .Include(p => p.CreatedBy)  
                    .ThenInclude(u => u.ProfilePicture)
                .Include(p => p.Tags)
                .Include(p => p.Reactions)
                .Include(p => p.Comments)
                .Include(p => p.TaggedUsers)
                .Include(p => p.Attachments)
                .Where(p =>
                    p.DeletedOn == null &&
                    !p.CreatedBy.BlockedUsers.Select(bu => bu.Id).Contains(currentUser.Id) &&
                    (
                        p.Visibility.Equals("all") ||
                        (p.Visibility.Equals("friends") && p.CreatedBy.Friends.Any(f => f.Id == currentUser.Id)) ||
                        (p.Visibility.Equals("followers") && p.CreatedBy.Followers.Any(f => f.Id == currentUser.Id)) ||
                        (p.CreatedBy.IsPrivate && p.CreatedBy.Friends.Any(f => f.Id == currentUser.Id)) ||
                        (p.CreatedBy.Id == currentUser.Id)
                    ))
                .ToListAsync();

           
            var posts = allPosts
                .Where(p => (p.Description != null && p.Description.ToLower().Contains(query)) ||
                            (p.Tags != null && p.Tags.Any(t => t.Name != null && t.Name.ToLower().Contains(query))))
                .OrderByDescending(p => p.Tags != null && p.Tags.Any(t => t.Name != null && t.Name.ToLower() == query))
                .ThenByDescending(p => p.Description != null && p.Description.ToLower().Contains(query))
                .ThenByDescending(p => p.CreatedOn)
                .ToList();

            var userViewModels = orderedUsers.Select(u => new SearchUserViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                ProfilePictureUrl = u.ProfilePicture?.CloudUrl,
                IsFriend = currentUser.Friends != null && currentUser.Friends.Any(f => f.Id == u.Id),
                IsFriendRequestSent = u.ReceivedFriendRequests != null && u.ReceivedFriendRequests.Any(r => r.CreatedById == currentUser.Id),
                IsFriendRequestReceived = currentUser.ReceivedFriendRequests != null && currentUser.ReceivedFriendRequests.Any(r => r.CreatedById == u.Id),
                IsFollowing = currentUser.Following != null && currentUser.Following.Any(f => f.Id == u.Id)
            }).ToList();

            var postViewModels = posts.Select(p => new SearchedPostsWebModel
            {
                Id = p.Id,
                Description = p.Description,
                AttachmentUrls = p.Attachments?.Select(a => a.CloudUrl).ToList() ?? new List<string>(),
                Tags = p.Tags?.Select(t => t.Name).ToList() ?? new List<string>(),
                UserName = p.CreatedBy.UserName,
                ProfilePictureUrl = p.CreatedBy.ProfilePicture?.CloudUrl,
                CreatedOn = p.CreatedOn,
                CreatedById = p.CreatedBy.Id,
                TaggedUsersId = p.TaggedUsers?.Select(u => u.Id).ToList() ?? new List<string>(),
                TaggedUsersUserNames = p.TaggedUsers?.Select(u => u.UserName).ToList() ?? new List<string>(),
                IsUserDeleted = p.CreatedBy.IsDeleted
            }).ToList();

            ViewData["Query"] = query;
            ViewData["ProfilePictureUrl"] = currentUser.ProfilePicture?.CloudUrl;
            ViewData["FriendRequests"] = currentUser.ReceivedFriendRequests?.ToList();
            ViewData["SavedPosts"] = currentUser.SavedPosts?.Select(p => p.Id).ToList() ?? new List<string>();

            var searchResultsViewModel = new SearchResultsViewModel
            {
                Query = query,
                Users = userViewModels,
                Posts = postViewModels
            };

            return View("SearchResults", searchResultsViewModel);
        }



    }
}