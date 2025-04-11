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
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Service.Hub;

namespace SocialMedia.Service.Friends
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly FriendRequestRepository friendRequestRepository;
        private readonly SocialMediaUserRepository socialMediaUserRepository;
        private readonly NotificationRepository notificationRepository;
        private readonly IServiceProvider serviceProvider;
        public FriendRequestService(FriendRequestRepository friendRequestRepository,
            SocialMediaUserRepository socialMediaUserRepository,
            NotificationRepository notificationRepository,
            IServiceProvider serviceProvider)
        {
            this.friendRequestRepository = friendRequestRepository;
            this.socialMediaUserRepository = socialMediaUserRepository;
            this.notificationRepository = notificationRepository;
            this.serviceProvider = serviceProvider;
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

            if (receiver.BlockedUsers.Select(bu => bu.Id).Contains(sender.Id))
            {
                throw new Exception("You cannot send a friend request to a user who has blocked you.");
            }

            else if (sender.BlockedUsers.Select(bu => bu.Id).Contains(receiver.Id))
            {
                throw new Exception("You cannot send a friend request to a user you have blocked.");
            }
            else if (!ifRequestExists && isRequestValid)
            {
                var request = await friendRequestRepository.CreateAsync(new FriendRequest { Receiver = receiver, Status = "Pending" });
                var notification = new Notification
                {
                    UserId = request.ReceiverId,
                    Message = $"{request.CreatedBy.UserName} sent you a friend request",
                    CreatedAt = DateTime.UtcNow
                };
                await notificationRepository.AddNotificationAsync(notification);

                var hubContext = serviceProvider.GetRequiredService<IHubContext<NotificationHub>>();
                await hubContext.Clients.User(request.CreatedById).SendAsync("ReceiveNotification", notification.Message, notification.CreatedAt);
                return request.ToModel();
            }

            else if (ifRequestExists)
            {
                throw new Exception("A friend request already exists between these users.");
            }

            else if (!isRequestValid)
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
            var notification = new Notification
            {
                UserId = request.ReceiverId,
                Message = $"{request.CreatedBy.UserName} accepted your friend request",
                CreatedAt = DateTime.UtcNow
            };
            await notificationRepository.AddNotificationAsync(notification);

            var hubContext = serviceProvider.GetRequiredService<IHubContext<NotificationHub>>();
            await hubContext.Clients.User(request.CreatedById).SendAsync("ReceiveNotification", notification.Message, notification.CreatedAt);

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
            var notification = new Notification
            {
                UserId = request.ReceiverId,
                Message = $"{request.CreatedBy.UserName} denied your friend request",
                CreatedAt = DateTime.UtcNow
            };
            await notificationRepository.AddNotificationAsync(notification);

            var hubContext = serviceProvider.GetRequiredService<IHubContext<NotificationHub>>();
            await hubContext.Clients.User(request.CreatedById).SendAsync("ReceiveNotification", notification.Message, notification.CreatedAt);
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

            else if (following.IsPrivate)
            {
                throw new Exception("You cannot follow a user whose account is private.");
            }

            else if(following.BlockedUsers.Select(bu => bu.Id).Contains(user.Id))
            {
                throw new Exception("You cannot follow a user who has blocked you.");
            }

            else
            {
                user.Following.Add(following);
                following.Followers.Add(user);
                await socialMediaUserRepository.UpdateAsync(user);
                await socialMediaUserRepository.UpdateAsync(following);
                var notification = new Notification
                {
                    UserId = following.Id,
                    Message = $"{user.UserName} started following you",
                    CreatedAt = DateTime.UtcNow
                };
                await notificationRepository.AddNotificationAsync(notification);

                var hubContext = serviceProvider.GetRequiredService<IHubContext<NotificationHub>>();
                await hubContext.Clients.User(following.Id).SendAsync("ReceiveNotification", notification.Message, notification.CreatedAt);

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

        public async Task<bool> Block(SocialMediaUser user, SocialMediaUser blocking)
        {
            if (user is null || blocking is null)
            {
                throw new Exception("User cannot be null");
            }

            if(user.BlockedUsers.Select(u => u.Id).Contains(blocking.Id))
            {
                throw new Exception("Ypu have already blocked the user");
            }
            if(user.Friends.Select(f => f.Id).Contains(blocking.Id))
            {
                user.Friends.Remove(blocking);
                blocking.Friends.Remove(user);
            }

            if (user.Followers.Select(f => f.Id).Contains(blocking.Id))
            {
                user.Followers.Remove(blocking);
                blocking.Following.Remove(user);
            }

            var request = user.ReceivedFriendRequests.FirstOrDefault(fr => fr.CreatedById == blocking.Id);
            user.ReceivedFriendRequests.Remove(request);
            blocking.SentFriendRequests.Remove(request);
            if(request is not null)
            {
                await friendRequestRepository.HardDeleteAsync(request);
            }
            
            user.BlockedUsers.Add(blocking);
            await socialMediaUserRepository.UpdateAsync(user);
            await socialMediaUserRepository.UpdateAsync(blocking);
            return true;
        }

        public async Task<bool> Unblock(SocialMediaUser user, SocialMediaUser unblocking)
        {
            if (user is null || unblocking is null)
            {
                throw new Exception("User cannot be null");
            }

            if (user.BlockedUsers.Select(f => f.Id).Contains(unblocking.Id))
            {
                user.BlockedUsers.Remove(unblocking);
            }

            else
            {
                throw new Exception("You have not blocked the target user!");
            }
            await socialMediaUserRepository.UpdateAsync(user);
            await socialMediaUserRepository.UpdateAsync(unblocking);
            return true;
        }
    }
}
