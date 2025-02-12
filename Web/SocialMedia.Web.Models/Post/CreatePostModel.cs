using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Web.Models.Utilities.Binding;

namespace SocialMedia.Web.Models.Post
{
    public class CreatePostModel
    {
        public string Description { get; set; }

        public List<IFormFile> Attachments { get; set; } = new List<IFormFile>();

        [BindProperty(BinderType = typeof(TagsModelBinder))]
        public List<string> Tags { get; set; }

    }
}
