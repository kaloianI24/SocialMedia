using Azure;
using Microsoft.Extensions.Hosting;
using SocialMedia.Web.Models.Post;

namespace SocialMedia.Models
{
    public class SearchResultsViewModel
    {
        public string Query { get; set; }
        public List<SearchUserViewModel> Users { get; set; } = new List<SearchUserViewModel>();
        public List<SearchedPostsWebModel> Posts { get; set; } = new List<SearchedPostsWebModel>();
    }

    public class SearchUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public bool IsFriend { get; set; }
        public bool IsFriendRequestSent { get; set; }
        public bool IsFriendRequestReceived { get; set; }
        public bool IsFollowing { get; set; }
    }
}
