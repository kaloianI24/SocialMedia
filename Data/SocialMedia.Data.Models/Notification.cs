using SocialMedia.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Data.Models
{
    public class Notification : BaseEntity
    {
        public string UserId { get; set; }
        public SocialMediaUser User { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
