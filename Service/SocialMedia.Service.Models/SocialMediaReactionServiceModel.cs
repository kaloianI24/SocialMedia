using SocialMedia.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Service.Models
{
    public class SocialMediaReactionServiceModel : MetadataBaseServiceModel
    {
        public string Label { get; set; }

        public CloudResourceServiceModel Emote { get; set; }
    }
}
