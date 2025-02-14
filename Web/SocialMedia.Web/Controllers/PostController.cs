using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Areas.Identity.Data;
using SocialMedia.Service.Cloud;
using SocialMedia.Service.Models;
using SocialMedia.Service.Post;
using SocialMedia.Web.Models.Post;
using System.Net.Mail;
using SocialMedia.Service.Mappings;
using static SocialMedia.Service.Mappings.SocialMediaPostMappings;
using System.Globalization;
using SocialMedia.Web.Models;
using SocialMedia.Web.Models.User;

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
            if(currentUserId == targetUserId)
            {
                ViewData["IsOwner"] = true;
            }
            else
            {
                ViewData["IsOwner"] = false;
            }
            ViewData["ProfilePictureUrl"] = currentUser?.ProfilePicture?.CloudUrl;
            return View(targetUser.ToModel(UserPostMappingsContext.User));
        }

        public async Task<IActionResult> LoadPartial(string type, string id)
        {
            var user = await GetUserById(id);
            var currentUser = await GetUser();

            switch (type)
            {
                case "MyPosts":
                    ViewData["ProfilePictureUrl"] = currentUser?.ProfilePicture?.CloudUrl;
                    return PartialView("_MyPosts", user.ToModel(UserPostMappingsContext.User));

                case "TaggedPosts":
                    var webModels = await TaggedPosts(user.Id);
                    ViewData["ProfilePictureUrl"] = currentUser?.ProfilePicture?.CloudUrl;
                    return PartialView("_TaggedPosts", webModels);

                default:
                    return BadRequest();
            }
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
                CreatedById = p.CreatedBy.Id
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
            .Include(u => u.TaggedPosts)
                .ThenInclude(p => p.Attachments)
            .Include(u => u.TaggedPosts)
                .ThenInclude(p => p.TaggedUsers)
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
            .Include(u => u.TaggedPosts)
                .ThenInclude(p => p.Attachments)
            .Include(u => u.TaggedPosts)
                .ThenInclude(p => p.TaggedUsers)
            .Include(u => u.Followers)
            .Include(u => u.Friends)
            .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
