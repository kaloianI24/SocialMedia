using SocialMedia.Areas.Identity.Data;
using SocialMedia.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Service.Models
{
    public class PostServiceModel : MetadataBaseServiceModel
    {
        public string Description { get; set; }

        public List<CloudResource> Attachments { get; set; }

        public List<SocialMediaUser> TaggedUsers { get; set; }

        public List<SocialMediaTag> Tags { get; set; }
        
        //public List<UserPostComment> Comments { get; set; }

        //public List<UserPostReaction> Reactions { get; set; }
    }
}
