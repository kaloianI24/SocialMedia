using SocialMedia.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Service.Models
{
    public class FriendRequestServiceModel
    {
        public string Id { get; set; }
        public string ReceiverId { get; set; }

        public string Status { get; set; }

        public string CreatedById { get; set; }
    }
}
