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
using SocialMedia.Data.Models;
using Microsoft.Extensions.Hosting;

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
            ViewData["IsAccountPrivate"] = currentUser.IsPrivate;
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
                Visibility = createPostModel.Visibility ?? "friends"
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
            var areFriends = currentUser.Friends.Any(f => f.Id == targetUser.Id);
            var isFollowing = currentUser.Following.Any(f => f.Id == targetUser.Id);
            List<TaggedPostWebModel> posts;

            switch (type)
            {
                case "MyPosts":
                    ViewData["IsOwner"] = (currentUserId == targetUserId);
                    var roles = await _userManager.GetRolesAsync(currentUser);
                    bool isAdmin = roles.Contains("Admin");
                    ViewData["IsAccountPrivate"] = targetUser.IsPrivate;
                    ViewData["IsAdmin"] = isAdmin;
                    ViewData["AreFriends"] = areFriends;
                    ViewData["IsFollowing"] = isFollowing;
                    ViewData["SavedPostsId"] = currentUser.SavedPosts.Select(p => p.Id).ToList();
                    
                    if(currentUserId == targetUserId || areFriends)
                    {
                        posts = ConvertFromServiceModelToWebModel(user.Posts.Where(p => p.DeletedOn == null).ToList());
                    }

                    else if(isFollowing)
                    {
                        posts = ConvertFromServiceModelToWebModel(user.Posts.Where(p => p.Visibility.Equals("all") || p.Visibility.Equals("followers") && p.DeletedOn == null).ToList());
                    }
                    else
                    {
                        posts = ConvertFromServiceModelToWebModel(user.Posts.Where(p => p.Visibility.Equals("all") && p.DeletedOn == null).ToList());
                    }                        

                    return PartialView("_MyPosts", posts);

                case "TaggedPosts":
                    ViewData["IsOwner"] = (currentUserId == targetUserId);
                    ViewData["AreFriends"] = areFriends;
                    ViewData["IsAccountPrivate"] = targetUser.IsPrivate;
                    ViewData["SavedPostsId"] = currentUser.SavedPosts.Select(p => p.Id).ToList();

                    if (currentUserId == targetUserId)
                    {
                        posts = ConvertFromServiceModelToWebModel(targetUser.TaggedPosts.Where(p => p.DeletedOn == null).ToList());
                    }
                    else
                    {
                        posts = ConvertFromServiceModelToWebModel(targetUser.TaggedPosts.Where(
                            p => p.DeletedOn == null &&
                            p.Visibility.Equals("all") ||
                            p.Visibility.Equals("friends") && p.CreatedBy.Friends.Contains(currentUser) ||
                            p.Visibility.Equals("followers") && p.CreatedBy.Followers.Contains(currentUser) || p.CreatedBy.Friends.Contains(currentUser) ||
                            p.CreatedBy.Id == currentUser.Id)
                        .ToList());
                    }
                    return PartialView("_TaggedPosts", posts);

                case "DeletedPosts":
                    ViewData["IsOwner"] = (currentUserId == targetUserId);
                    return PartialView("_DeletedPosts", ConvertFromServiceModelToWebModel(currentUser.Posts.Where(p => p.DeletedOn != null).ToList()).ToList());

                case "SavedPosts":
                    ViewData["IsOwner"] = (currentUserId == targetUserId);
                    ViewData["AreFriends"] = areFriends;
                    ViewData["IsFollowing"] = isFollowing;
                    ViewData["SavedPostsId"] = currentUser.SavedPosts.Select(p => p.Id).ToList();
                    return PartialView("_SavedPosts", ConvertFromServiceModelToWebModel(currentUser.SavedPosts.Where(
                        p => p.DeletedOn == null &&
                        p.Visibility.Equals("all") ||
                        p.Visibility.Equals("friends") && p.CreatedBy.Friends.Contains(currentUser) ||
                        p.Visibility.Equals("followers") && p.CreatedBy.Followers.Contains(currentUser) || p.CreatedBy.Friends.Contains(currentUser) ||
                        p.CreatedBy.Id == currentUser.Id).ToList()));

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
                Visibility = post.Visibility
            };
            var currentUser = await GetUser();
            ViewData["ProfilePictureUrl"] = currentUser?.ProfilePicture?.CloudUrl;
            ViewData["IsAccountPrivate"] = currentUser.IsPrivate;
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

        //private async Task<List<TaggedPostWebModel>> TaggedPosts(string userId)
        //{
        //    var currentUser = await GetUser();
        //    var currentUserId = currentUser.Id;
        //    var targetUserId = userId ?? currentUserId;
        //    var targetUser = await GetUserById(targetUserId);

        //    bool isOwner = (currentUserId == targetUserId);
        //    //var posts = _socialMediaPostService.GetAllTaggedPosts(targetUser, isOwner, currentUser).ToList();
        //    var posts = currentUser.TaggedPosts;
        //    var webModels = posts.Select(p => new TaggedPostWebModel
        //    {
        //        Id = p.Id,
        //        Description = p.Description,
        //        AttachmentUrls = p.Attachments.Select(a => a.CloudUrl).ToList(),
        //        Tags = p.Tags.Select(t => t.Name).ToList(),
        //        UserName = p.CreatedBy.UserName,
        //        ProfilePictureUrl = p.CreatedBy.ProfilePicture?.CloudUrl,
        //        CreatedOn = p.CreatedOn,
        //        CreatedById = p.CreatedBy.Id,
        //        TaggedUsersId = p.TaggedUsers.Select(p => p.Id).ToList(),
        //        TaggedUsersUserNames = p.TaggedUsers.Select(p => p.UserName).ToList(),
        //    }).ToList();

        //    return webModels;
        //}

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> SavePost([FromQuery] string postId)
        {
            try
            {
                var currentUser = await GetUser();
                var post = await _socialMediaPostService.SavePost(postId, currentUser);

                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
                        
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> UnsavePost([FromQuery] string postId)
        {
            try
            {
                var currentUser = await GetUser();
                var post = await _socialMediaPostService.UnsavePost(postId, currentUser);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

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
            .Include(u => u.TaggedPosts)
                .ThenInclude(p => p.CreatedBy)
                .ThenInclude(u => u.ProfilePicture)
            .Include(u => u.Following)
            .Include(u => u.Followers)
            .Include(u => u.Friends)
            .Include(u => u.SavedPosts)
                .ThenInclude(sp => sp.Attachments)
            .Include(u => u.SavedPosts)
                .ThenInclude(sp => sp.TaggedUsers)
            .Include(u => u.SavedPosts)
                .ThenInclude(sp => sp.Tags)
            .Include(u => u.SavedPosts)
                .ThenInclude(sp => sp.CreatedBy)
                    .ThenInclude(crb => crb.ProfilePicture)
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
            .Include(u => u.SavedPosts)
                .ThenInclude(sp => sp.Attachments)
            .Include(u => u.SavedPosts)
                .ThenInclude(sp => sp.TaggedUsers)
            .Include(u => u.SavedPosts)
                .ThenInclude(sp => sp.Tags)
             .Include(u => u.SavedPosts)
                .ThenInclude(sp => sp.CreatedBy)
                    .ThenInclude(crb => crb.ProfilePicture)
            .FirstOrDefaultAsync(u => u.Id == id);
        }

        private List<TaggedPostWebModel> ConvertFromServiceModelToWebModel(List<SocialMediaPost> posts)
        {
            return posts.Select(p => new TaggedPostWebModel
            {
                Id = p.Id,
                Description = p.Description,
                AttachmentUrls = p.Attachments.Select(a => a.CloudUrl).ToList(),
                UserName = p.CreatedBy.UserName,
                ProfilePictureUrl = p.CreatedBy.ProfilePicture.CloudUrl,
                CreatedOn = p.CreatedOn,
                CreatedById = p.CreatedBy.Id,
                Tags = p.Tags.Select(t => t.Name).ToList(),
                TaggedUsersId = p.TaggedUsers.Select(u => u.Id).ToList(),
                TaggedUsersUserNames = p.TaggedUsers.Select(u => u.UserName).ToList(),
            }).ToList();
        }
    }
}