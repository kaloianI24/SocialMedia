using SocialMedia.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Data.Models
{
    public class UserPostComment : BaseEntity
    {
        public SocialMediaUser User { get; set; }

        public SocialMediaPost Post { get; set; }

        public SocialMediaComment Comment { get; set; }
    }
}
