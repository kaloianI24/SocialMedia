using SocialMedia.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Data.Models
{
    public class UserCommentReaction : BaseEntity
    {
        public SocialMediaUser User { get; set; }

        public Comment Comment { get; set; }

        public Reaction Reaction { get; set; }
    }
}
