using Microsoft.AspNetCore.Mvc;
using SocialMedia.Service.Cloud;
using SocialMedia.Service.Models;
using SocialMedia.Service.Post;
using SocialMedia.Web.Models.Post;
using System.Net.Mail;

namespace SocialMedia.Controllers
{
    public class PostController : Controller
    {
        private readonly ISocialMediaPostService _socialMediaPostService;

        private readonly ICloudinaryService _cloudinaryService;

        public PostController(ISocialMediaPostService socialMediaPostService,
            ICloudinaryService cloudinaryService)
        {
            _socialMediaPostService = socialMediaPostService;
            _cloudinaryService = cloudinaryService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
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

            await _socialMediaPostService.CreateAsync(new PostServiceModel
            {
                Description = createPostModel.Description,
                Attachments = attachments,
                Tags = createPostModel.Tags.Select(tag => new TagServiceModel { Name = tag }).ToList(),
                
            });

            return RedirectToAction("MyPage", "Home");
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
    }
}
