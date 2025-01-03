using SocialMedia.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Data.Models
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }
        public List<Post> Posts { get; } = [];
    }
}
