using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Areas.Identity.Data;
using SocialMedia.Service.Cloud;
using SocialMedia.Service.Models;
using SocialMedia.Service.SocialMediaPost;
using SocialMedia.Web.Models.Post;
using System.Net.Mail;
using SocialMedia.Service.Mappings;
using static SocialMedia.Service.Mappings.SocialMediaPostMappings;
using System.Globalization;
using SocialMedia.Web.Models;
using SocialMedia.Web.Models.User;
using SocialMedia.Service.SocialMediaPost;

namespace SocialMedia.Controllers
{
    public class PostController : Controller
    {
        private readonly ISocialMediaPostService _socialMediaPostService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly UserManager<SocialMediaUser> _userManager;

        public PostController(ISocialMediaPostService socialMediaPostService,
            ICloudinaryService cloudinaryService,
            UserManager<SocialMediaUser> userManager)
        {
            _socialMediaPostService = socialMediaPostService;
            _cloudinaryService = cloudinaryService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var currentUser = await GetUser();
            this.ViewData["Users"] = _userManager.Users.Include(u => u.ProfilePicture).Where(u => u.Id != currentUser.Id).Select(u => new UserWebModel
            {
                Id = u.Id,
                UserName = u.UserName,
                ProfilePictureUrl = u.ProfilePicture.CloudUrl
            })
            .ToList();
            ViewData["ProfilePictureUrl"] = currentUser?.ProfilePicture?.CloudUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateConfirm(CreatePostModel createPostModel)
        {
            var photos = createPostModel.Attachments.ToArray();
            Dictionary<IFormFile, string> photosUrls = new Dictionary<IFormFile, string>();

            for (int i = 0; i < photos.Length; i++)
            {
                var photoUrl = await this.UploadPhoto(photos[i]);
                photosUrls.Add(photos[i], photoUrl);
            }

            var attachments = photosUrls.Select(photo => new CloudResourceServiceModel
            {
                CloudUrl = photo.Value
            }).ToList();

            var taggedUsersIds = createPostModel.TaggedUsersId?.Split(",").Where(id => id != null).ToList();

            await _socialMediaPostService.CreateAsync(new PostServiceModel
            {
                Description = createPostModel.Description,
                Attachments = attachments,
                Tags = createPostModel.Tags.Select(tag => new TagServiceModel { Name = tag }).ToList(),
                TaggedUsersId = taggedUsersIds,
            });

            return Redirect("MyPage");
        }

        public async Task<IActionResult> MyPage(string? userId)
        {
            var currentUser = await GetUser();
            var currentUserId = currentUser.Id;
            var targetUserId = userId ?? currentUserId;
            var targetUser = await GetUserById(targetUserId);

            ViewData["IsOwner"] = (currentUserId == targetUserId);
            if(currentUserId != targetUserId)
            {
                var areFriends = currentUser.Friends.Any(f => f.Id == targetUser.Id);
                ViewData["AreFriends"] = areFriends;
                var isFolowing = currentUser.Following.Any(f => f.Id == targetUser.Id);
                ViewData["IsFollowing"] = isFolowing;
            }
            ViewData["ProfilePictureUrl"] = currentUser?.ProfilePicture?.CloudUrl;
            return View(targetUser.ToModel(UserPostMappingsContext.User));
        }

        public async Task<IActionResult> LoadPartial(string type, string id)
        {
            var user = await GetUserById(id);
            var currentUser = await GetUser();
            var currentUserId = currentUser.Id;
            var targetUserId = id ?? currentUserId;
            var targetUser = await GetUserById(targetUserId);

            switch (type)
            {
                case "MyPosts":
                    ViewData["IsOwner"] = (currentUserId == targetUserId);
                    return PartialView("_MyPosts", user.ToModel(UserPostMappingsContext.User));

                case "TaggedPosts":
                    ViewData["IsOwner"] = (currentUserId == targetUserId);
                    var webModels = await TaggedPosts(user.Id);
                    return PartialView("_TaggedPosts", webModels);

                case "DeletedPosts":
                    ViewData["IsOwner"] = (currentUserId == targetUserId);
                    return PartialView("_DeletedPosts", user.ToModel(UserPostMappingsContext.User));

                default:
                    return BadRequest();
            }
        }


        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> DeletePost([FromQuery] string postId)
        {
            var post = await _socialMediaPostService.DeleteAsync(postId);

            return NoContent();
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Recover([FromQuery] string postId)
        {
            var post = await _socialMediaPostService.RecoverAsync(postId);

            return NoContent();
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> RemoveTaggedUser([FromQuery] string postId)
        {
            var currentUser = await GetUser();
            await _socialMediaPostService.RemoveTaggedUser(currentUser.Id, postId);
            return NoContent();
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> DeletePermanently([FromQuery] string postId)
        {
            var post = await _socialMediaPostService.DeletePermanentlyAsync(postId);

            return NoContent();
        }

        [HttpGet]
        [Consumes("application/json")]
        public async Task<IActionResult> Update([FromQuery] string postId)
        {
            var post = await _socialMediaPostService.GetByIdAsync(postId);
            var postWebModel = new UpdatePostWebModel
            {
                Id = post.Id,
                Description = post.Description,
                Attachments = post.Attachments,
                TaggedUsersId = post.TaggedUsersId,
                TaggedUsersUserName = post.TaggedUsersUserName,
                Tags = string.Join(",",post.Tags.Select(t => t.Name)),
                RemovedAttachmentIds = null,
            };
            var currentUser = await GetUser();
            ViewData["Users"] = _userManager.Users.Include(u => u.ProfilePicture).Where(u => u.Id != currentUser.Id).Select(u => new UserWebModel
            {
                Id = u.Id,
                UserName = u.UserName,
                ProfilePictureUrl = u.ProfilePicture.CloudUrl
            })
           .ToList();
            return View(postWebModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateConfirm(UpdatePostWebModel post)
        {
            await _socialMediaPostService.UpdateAsync(post);
            return Redirect("MyPage");
        }

        private async Task<List<TaggedPostWebModel>> TaggedPosts(string userId)
        {
            var posts = _socialMediaPostService.GetAllTaggedPosts(userId).ToList();
            var webModels = posts.Select(p => new TaggedPostWebModel
            {
                Id = p.Id,
                Description = p.Description,
                AttachmentUrls = p.Attachments.Select(a => a.CloudUrl).ToList(),
                Tags = p.Tags.Select(t => t.Name).ToList(),
                UserName = p.CreatedBy.UserName,
                ProfilePictureUrl = p.CreatedBy.ProfilePicture?.CloudUrl,
                CreatedOn = p.CreatedOn,
                CreatedById = p.CreatedBy.Id,
                TaggedUsersId = p.TaggedUsersId.ToList(),
                TaggedUsersUserNames = p.TaggedUsersUserName.ToList(),
            }).ToList();

            return webModels;
        }

        private async Task<string> UploadPhoto(IFormFile photo)
        {
            var uploadResponse = await _cloudinaryService.UploadFile(photo);

            if (uploadResponse == null)
            {
                return null;
            }

            return uploadResponse["url"].ToString();
        }

        private Task<SocialMediaUser> GetUser()
        {
            return _userManager.Users
            .Include(u => u.ProfilePicture)
            .Include(u => u.Posts)
                .ThenInclude(p => p.Attachments)
             .Include(u => u.Posts)
                .ThenInclude(p => p.Tags)
             .Include(u => u.Posts)
                .ThenInclude(p => p.TaggedUsers)
             .Include(u => u.Posts)
                .ThenInclude(p => p.DeletedBy)
            .Include(u => u.TaggedPosts)
                .ThenInclude(p => p.Attachments)
            .Include(u => u.TaggedPosts)
                .ThenInclude(p => p.TaggedUsers)
            .Include(u => u.Following)
            .Include(u => u.Followers)
            .Include(u => u.Friends)
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
             .Include(u => u.Posts)
                .ThenInclude(p => p.DeletedBy)
            .Include(u => u.Posts)
                .ThenInclude(p => p.TaggedUsers)
            .Include(u => u.TaggedPosts)
                .ThenInclude(p => p.Attachments)
            .Include(u => u.TaggedPosts)
                .ThenInclude(p => p.TaggedUsers)
            .Include(u => u.Following)
            .Include(u => u.Followers)
            .Include(u => u.Friends)
            .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
