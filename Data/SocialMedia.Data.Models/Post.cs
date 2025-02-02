using SocialMedia.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Data.Models
{
    public class Post : MetadataBaseEntity
    {
        public string Description { get; set; }

        public List<CloudResource> Attachments { get; set; } = new List<CloudResource>();

        public List<UserPostComment> Comments { get; set; } = new List<UserPostComment>();

        public List<UserPostReaction> Reactions { get; set; } = new List<UserPostReaction>();

        public List<SocialMediaUser>? TaggedUsers { get; set; } = new List<SocialMediaUser>();

        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
