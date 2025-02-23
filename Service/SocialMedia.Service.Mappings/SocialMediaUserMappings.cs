using SocialMedia.Areas.Identity.Data;
using SocialMedia.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SocialMedia.Service.Mappings.SocialMediaPostMappings;

namespace SocialMedia.Service.Mappings
{
    public static class SocialMediaUserMappings
    {
        public static SocialMediaUser ToEntity(this SocialMediaUserServiceModel model)
        {
            return new SocialMediaUser();
        }

        public static SocialMediaUserServiceModel ToModel(this SocialMediaUser entity, UserPostMappingsContext context)
        {
            if(entity is null)
            {
                return null;
            }
            return new SocialMediaUserServiceModel
            {
                Id = entity.Id,
                Role = entity.Role?.ToModel(),
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                UserName = entity.UserName,
                Email = entity.Email,
                ProfilePicture = entity.ProfilePicture.ToModel(),
                IsPrivate = entity.IsPrivate,
                Posts = ShouldMapPost(context) ? entity.Posts?.Select(post => post.ToModel(UserPostMappingsContext.User)).ToList() : null,
                Friends = entity.Friends?.Select(friend => friend.ToModelBasic()).ToList(),
                Followers = entity.Followers?.Select(f => f.ToModelBasic()).ToList(),
                Following = entity.Following?.Select(f => f.ToModelBasic()).ToList(),
            };
        }

        public static SocialMediaUserBasicServiceModel ToModelBasic(this SocialMediaUser entity)
        {
            if (entity is null)
            {
                return null;
            }
            return new SocialMediaUserBasicServiceModel
            {
                Id = entity.Id,
                UserName = entity.UserName,
                ProfilePicture = entity.ProfilePicture.ToModel(),
            };
        }

    }
}
