using SocialMedia.Areas.Identity.Data;
using SocialMedia.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Service.Models
{
    public class UserPostReactionServiceModel : BaseServiceModel
    {
        public SocialMediaUserServiceModel User { get; set; }

        public PostServiceModel Post { get; set; }

        public SocialMediaReactionServiceModel Reaction { get; set; }
    }
}
