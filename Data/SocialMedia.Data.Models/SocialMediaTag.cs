using SocialMedia.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Data.Models
{
    public class SocialMediaTag : MetadataBaseEntity
    {
        public string Name { get; set; }
        public List<SocialMediaPost> Posts { get; } = [];
    }
}
