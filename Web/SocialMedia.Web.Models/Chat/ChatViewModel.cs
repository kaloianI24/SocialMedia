using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Web.Models.Chat
{
    public class ChatViewModel
    {
        public string ReceiverId { get; set; }
        public string ReceiverUserName { get; set; }
        public string CurrentUserId { get; set; }
        public List<ChatMessageBasic> Messages { get; set; }
    }
}
