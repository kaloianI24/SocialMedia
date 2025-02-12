using SocialMedia.Areas.Identity.Data;
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
                Posts = ShouldMapPost(context) ? entity.Posts?.Select(post => post.ToModel(UserPostMappingsContext.User)).ToList() : null,
                
            };
        }
    }
}
