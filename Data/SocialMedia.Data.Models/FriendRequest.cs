using SocialMedia.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Data.Models
{
    public class FriendRequest : MetadataBaseEntity
    {
        public const string defaultStatus = "Pending";
        public string ReceiverId { get; set; }
        public SocialMediaUser Receiver { get; set; }
        public string Status { get; set; } = defaultStatus;
    }
}
