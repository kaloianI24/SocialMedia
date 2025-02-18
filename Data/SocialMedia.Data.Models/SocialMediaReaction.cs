using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Data.Models
{
    public class SocialMediaReaction : MetadataBaseEntity
    {
        public string Label { get; set; }

        public CloudResource Emote { get; set; }
    }
}
