using SocialMedia.Areas.Identity.Data;
using SocialMedia.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Service.Friends
{
    public interface IFriendRequestService
    {
        public Task<FriendRequestServiceModel> CreateFriendRequest(SocialMediaUser receiver, SocialMediaUser sender);
        public Task<FriendRequestServiceModel> AcceptFriendRequest(string requestId);
        public Task<FriendRequestServiceModel> DeleteFriendRequest(string requestId);
        public Task<bool> RemoveFriend(SocialMediaUser user, SocialMediaUser friend);
        public Task<bool> Follow(SocialMediaUser user, SocialMediaUser following);
        public Task<bool> Unfollow(SocialMediaUser user, SocialMediaUser unfollowing);
    }
}
