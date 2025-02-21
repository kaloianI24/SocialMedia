using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Service.Models
{
    public class SocialMediaUserBasicServiceModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public CloudResourceServiceModel? ProfilePicture { get; set; }
    }
}
