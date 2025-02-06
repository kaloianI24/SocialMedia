using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Service.Models
{
    public class PostServiceModel : MetadataBaseServiceModel
    {
        public List<SocialMediaTag> Tags { get; set; } = new List<SocialMediaTag>();
    }
}
