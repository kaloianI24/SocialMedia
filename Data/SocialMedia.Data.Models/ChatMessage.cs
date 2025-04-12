using SocialMedia.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Data.Models
{
    public class ChatMessage : BaseEntity
    {
        public string SenderId { get; set; }
        public SocialMediaUser Sender { get; set; }
        public string ReceiverId { get; set; }
        public SocialMediaUser Receiver { get; set; }
        public string EncryptedText { get; set; }
        public string IV { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; } = false;
    }
}
