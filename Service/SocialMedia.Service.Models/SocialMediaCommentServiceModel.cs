using SocialMedia.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Service.Models
{
    public class SocialMediaCommentServiceModel : MetadataBaseServiceModel
    {
        public string Content { get; set; }

        public List<CloudResourceServiceModel> Attachments { get; set; }

        public List<UserCommentReactionServiceModel> Reactions { get; set; }

        public List<SocialMediaCommentServiceModel> Replies { get; set; }

        public SocialMediaCommentServiceModel? Parent { get; set; }
    }
}
