using SocialMedia.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Data.Models
{
    public class UserPostReaction : MetadataBaseEntity
    {
        public SocialMediaUser User { get; set; }

        public Post Post { get; set; }

        public Reaction Reaction { get; set; }
    }
}

