using SocialMedia.Data.Models;
using SocialMedia.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Service.Mappings
{
    public static class SocialMediaPostMappings
    {
        public enum UserPostMappingsContext
        {
            Post,
            User,
            Tag
        }
        public static SocialMediaPost ToEntity(this PostServiceModel model)
        {
            return new SocialMediaPost
            {
                Description = model.Description,
                Attachments = model.Attachments.Select(attachment => attachment.ToEntity()).ToList(),
                Tags = model.Tags?.Select(tag => tag.ToEntity()).ToList(),
                TaggedUsers = model.TaggedUsers?.Select(user => user.ToEntity()).ToList(),
            };
        }

        public static PostServiceModel ToModel(this SocialMediaPost entity, UserPostMappingsContext context)
        {
            return new PostServiceModel
            {
                Id = entity.Id,
                Description = entity.Description,
                Attachments = entity.Attachments
                .Select(attachment => attachment.ToModel())
                .ToList(),
                Tags = entity.Tags?.Select(tag => tag.ToModel(UserPostMappingsContext.Post)).ToList(),
                TaggedUsers = entity.TaggedUsers?.Select(user => user.ToModel(UserPostMappingsContext.User)).ToList(),
                CreatedOn = entity.CreatedOn,
                UpdatedOn = entity.UpdatedOn,
                DeletedOn = entity.DeletedOn,
                CreatedBy = ShouldMapUser (context)? entity.CreatedBy.ToModel(UserPostMappingsContext.Post) : null,
                UpdatedBy = ShouldMapUser(context) ? entity.UpdatedBy?.ToModel(UserPostMappingsContext.Post) : null,
                DeletedBy = ShouldMapUser(context) ? entity.DeletedBy?.ToModel(UserPostMappingsContext.Post) : null,
            };
        }

        public static bool ShouldMapPost(UserPostMappingsContext context)
        {
            return context == UserPostMappingsContext.User;
        }

        public static bool ShouldMapUser(UserPostMappingsContext context)
        {
            return context == UserPostMappingsContext.Post;
        }
    }
}
