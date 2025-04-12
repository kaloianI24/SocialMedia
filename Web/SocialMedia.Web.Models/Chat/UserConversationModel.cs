using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Web.Models.Chat
{
    public class UserConversationModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateTime LastMessageDate { get; set; }
        public bool HasUnreadMessages { get; set; }
    }

}
