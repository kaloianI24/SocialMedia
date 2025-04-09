using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Web.Models.Chat
{
    public class ChatMessageBasic
    {
        public string SenderId { get; set; }
        public string PlainText { get; set; }
        public DateTime SentAt { get; set; }
    }
}
