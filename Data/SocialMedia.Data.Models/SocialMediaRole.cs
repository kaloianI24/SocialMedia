using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Data.Models
{
    public class SocialMediaRole : MetadataBaseEntity
    {
        public const string defaultRole = "User";

        // An admin must have some kind of visual effect on the front end to indicate he/she has admin privileges 
        // for example, different border over profile picture.
        public string Color { get; set; }

        public string Authority { get; set; } = defaultRole;
    }
}
