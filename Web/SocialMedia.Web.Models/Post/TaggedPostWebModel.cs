using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Web.Models.Utilities.Binding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Web.Models.Post
{
    public class TaggedPostWebModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public List<string> AttachmentUrls { get; set; } = new List<string>();
        public List<string> Tags { get; set; } = new List<string>();
        public string UserName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedById { get; set; }
    }
}
