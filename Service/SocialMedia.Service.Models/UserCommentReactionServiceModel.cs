using SocialMedia.Areas.Identity.Data;
using SocialMedia.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Service.Models
{
    public class UserCommentReactionServiceModel : BaseServiceModel
    {
        public SocialMediaUserServiceModel User { get; set; }

        public SocialMediaCommentServiceModel Comment { get; set; }

        public SocialMediaReactionServiceModel Reaction { get; set; }
    }
}
