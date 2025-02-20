using Microsoft.AspNetCore.Mvc;
using SocialMedia.Service.Cloud;
using SocialMedia.Service.Models;
using SocialMedia.Service.Reaction;
using SocialMedia.Web.Models.Reaction;
using SocialMedia.Service.Cloud;

namespace SocialMedia.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class ReactionController : Controller
    {
        private readonly IReactionService reactionService;

        private readonly ICloudinaryService cloudinaryService;

        public ReactionController(IReactionService reactionService, ICloudinaryService cloudinaryService)
        {
            this.reactionService = reactionService;
            this.cloudinaryService = cloudinaryService;
        }
        public IActionResult Index()
        {
            return View(this.reactionService.GetAll().ToList());
        }

        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateConfirm(CreateReactionModel model)
        {
            var reaction = await this.UploadPhoto(model.Emote);

            await this.reactionService.CreateAsync(new SocialMediaReactionServiceModel
            {
                Label = model.Label,
                Emote = new CloudResourceServiceModel { CloudUrl = reaction }
            });
            return Redirect("/Administration/Reaction"); 
        }

        private async Task<string> UploadPhoto(IFormFile photo)
        {
            var uploadResponse = await this.cloudinaryService.UploadFile(photo);

            if (uploadResponse == null)
            {
                return null;
            }

            return uploadResponse["url"].ToString();
        }
    }
}
