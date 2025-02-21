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

        public List<CloudResourceServiceModel> Attachments { get; set; }

        public List<string>? TaggedUsersId { get; set; }
        public List<string>? TaggedUsersUserName { get; set; }

        public List<SocialMediaUserServiceModel> TaggedUsers { get; set; }

        public List<TagServiceModel>? Tags { get; set; }
        
        public List<UserPostComment> Comments { get; set; }

        public List<UserPostReaction> Reactions { get; set; }
    }
}
