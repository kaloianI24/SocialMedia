using SocialMedia.Areas.Identity.Data;
using SocialMedia.Data.Models;
using SocialMedia.Data.Repositories;
using SocialMedia.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMedia.Service.Mappings;
using Microsoft.EntityFrameworkCore;
using Azure.Core;
using System.Reflection;

namespace SocialMedia.Service.Friends
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly FriendRequestRepository friendRequestRepository;
        private readonly SocialMediaUserRepository socialMediaUserRepository;
        public FriendRequestService(FriendRequestRepository friendRequestRepository,
            SocialMediaUserRepository socialMediaUserRepository)
        {
            this.friendRequestRepository = friendRequestRepository;
            this.socialMediaUserRepository = socialMediaUserRepository;
        }

        public async Task<FriendRequestServiceModel> CreateFriendRequest(SocialMediaUser receiver, SocialMediaUser sender)
        {
            if (receiver is null || sender is null)
            {
                throw new Exception("User cannot be null");
            }

            var ifRequestExists = friendRequestRepository.GetAll()
                .Include(r => r.CreatedBy)
                    .ThenInclude(u => u.Friends)
                .Include(r => r.Receiver)
                    .ThenInclude(u => u.Friends)
                 .Any(r => r.CreatedById == receiver.Id && r.ReceiverId == sender.Id || r.CreatedById == sender.Id && r.ReceiverId == receiver.Id);

            var isRequestValid = receiver.Id != sender.Id;
                      
            if (!ifRequestExists && isRequestValid)
            {
                var request = await friendRequestRepository.CreateAsync(new FriendRequest { Receiver = receiver, Status = "Pending" });
                return request.ToModel();
            }

            else if(ifRequestExists)
            {
                throw new Exception("A friend request already exists between these users.");
            }

            else if(!isRequestValid)
            {
                throw new Exception("You cannot be friend with yourself.");
            }

            else
            {
                throw new Exception("Something went wrong!");
            }
        }

        public async Task<FriendRequestServiceModel> AcceptFriendRequest(string requestId)
        {
            var request = friendRequestRepository.GetAll()
                .Include(r => r.CreatedBy)
                    .ThenInclude(u => u.Friends)
                 .Include(r => r.CreatedBy)
                    .ThenInclude(u => u.Followers)
                 .Include(r => r.CreatedBy)
                    .ThenInclude(u => u.Following)
                .Include(r => r.Receiver)
                    .ThenInclude(u => u.Friends)
                .Include(r => r.Receiver)
                    .ThenInclude(u => u.Followers)
                 .Include(r => r.Receiver)
                    .ThenInclude(u => u.Following)
                .FirstOrDefault(r => r.Id == requestId);

            request.Status = "Accepted";
            var areTheyFollowed = request.Receiver.Following.Any(f => f.Id == request.CreatedBy.Id);
            if (areTheyFollowed)
            {
                request.Receiver.Following.Remove(request.CreatedBy);
                request.CreatedBy.Followers.Remove(request.Receiver);
                await socialMediaUserRepository.UpdateAsync(request.Receiver);
                await socialMediaUserRepository.UpdateAsync(request.CreatedBy);
            }

            var areTheyFollowing = request.CreatedBy.Following.Any(f => f.Id == request.Receiver.Id);
            if (areTheyFollowing)
            {
                request.CreatedBy.Following.Remove(request.Receiver);
                request.Receiver.Followers.Remove(request.CreatedBy);
                await socialMediaUserRepository.UpdateAsync(request.Receiver);
                await socialMediaUserRepository.UpdateAsync(request.CreatedBy);
            }
            request.CreatedBy.Friends.Add(request.Receiver);
            request.Receiver.Friends.Add(request.CreatedBy);

            await socialMediaUserRepository.UpdateAsync(request.CreatedBy);
            await socialMediaUserRepository.UpdateAsync(request.Receiver);
            await friendRequestRepository.UpdateAsync(request);
            return request.ToModel();
        }

        public async Task<FriendRequestServiceModel> DeleteFriendRequest(string requestId)
        {
            var request = friendRequestRepository.GetAll()
                .Include(r => r.CreatedBy)
                    .ThenInclude(u => u.Friends)
                .Include(r => r.Receiver)
                    .ThenInclude(u => u.Friends)
                .FirstOrDefault(r => r.Id == requestId);

            request.Status = "Declined";

            await friendRequestRepository.UpdateAsync(request);
            return request.ToModel();
        }

        public async Task<bool> RemoveFriend(SocialMediaUser user, SocialMediaUser friend)
        {
            if (user is null || friend is null)
            {
                throw new Exception("User cannot be null");
            }

            var request = friendRequestRepository.GetAll()
                .Include(r => r.CreatedBy)
                    .ThenInclude(u => u.Friends)
                .Include(r => r.Receiver)
                    .ThenInclude(u => u.Friends)
                .FirstOrDefault(r => r.CreatedById == user.Id || r.CreatedById == friend.Id && r.ReceiverId == user.Id || r.ReceiverId == friend.Id);
            if(request == null)
            {
                throw new Exception("You are not friends do you cannot delete the relationship");
            }
            else
            {
                await friendRequestRepository.HardDeleteAsync(request);
                user.Friends.Remove(friend);
                friend.Friends.Remove(user);
                await socialMediaUserRepository.UpdateAsync(user);
                await socialMediaUserRepository.UpdateAsync(friend);
            }                
            return true;
        }

        public async Task<bool> Follow(SocialMediaUser user, SocialMediaUser following)
        {
            if (user is null || following is null)
            {
                throw new Exception("User cannot be null");
            }

            bool areFriends = user.Friends.Any(u => u.Id == following.Id);
            if(areFriends)
            {
                throw new Exception("You cannot follow a user how is already your friend");
            }

            if(following.IsPrivate)
            {
                throw new Exception("You cannot follow a user whose account is private.");
            }
            else
            {
                user.Following.Add(following);
                following.Followers.Add(user);
                await socialMediaUserRepository.UpdateAsync(user);
                await socialMediaUserRepository.UpdateAsync(following);
                return true;
            }
        }

        public async Task<bool> Unfollow(SocialMediaUser user, SocialMediaUser unfollowing)
        {
            if(user is null || unfollowing is null)
            {
                throw new Exception("User cannot be null");
            }

            var isFollowing = user.Following.Any(u => u.Id == unfollowing.Id);
            if(!isFollowing)
            {
                throw new Exception("You don't follow the user!");
            }
            else
            {
                user.Following.Remove(unfollowing);
                unfollowing.Followers.Remove(user);
            }

            bool areFriends = user.Friends.Any(u => u.Id == unfollowing.Id);
            if (areFriends)
            {
                user.Friends.Remove(unfollowing);
                unfollowing.Friends.Remove(user);
                user.Followers.Add(unfollowing);
                unfollowing.Following.Add(user);
            }
            
            await socialMediaUserRepository.UpdateAsync(user);
            await socialMediaUserRepository.UpdateAsync(unfollowing);
            return true;
        }
    }
}
