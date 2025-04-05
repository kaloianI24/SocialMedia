using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Web.Models.Post
{
    public class SearchedPostsWebModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public List<string> AttachmentUrls { get; set; } = new List<string>();
        public List<string> Tags { get; set; } = new List<string>();
        public string UserName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedById { get; set; }

        public List<string> TaggedUsersUserNames { get; set; } = new List<string>();
        public List<string> TaggedUsersId { get; set; } = new List<string>();

        public bool IsUserDeleted { get; set; }
    }
}
