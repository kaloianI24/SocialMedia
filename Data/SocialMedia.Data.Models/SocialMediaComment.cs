using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Data.Models
{
    public class SocialMediaComment : MetadataBaseEntity
    {
        public string Content { get; set; }

        public List<CloudResource> Attachments { get; set; }

        public List<UserCommentReaction> Reactions { get; set; }

        public List<SocialMediaComment> Replies { get; set; }

        public SocialMediaComment Parent { get; set; }
    }
}
