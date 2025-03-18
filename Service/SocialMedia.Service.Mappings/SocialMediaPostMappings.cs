using SocialMedia.Data.Models;
using SocialMedia.Service.Models;

namespace SocialMedia.Service.Mappings
{
    public static class SocialMediaPostMappings
    {
        public enum UserPostMappingsContext
        {
            Post,
            User,
            Reaction,
            Friend
        }
        public static SocialMediaPost ToEntity(this PostServiceModel model)
        {
            return new SocialMediaPost
            {
                Description = model.Description,
                Attachments = model.Attachments.Select(attachment => attachment.ToEntity()).ToList(),
                Visibility = model.Visibility,
                Tags = model.Tags?.Select(tag => tag.ToEntity()).ToList(),
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
                Tags = entity.Tags?.Select(tag => tag.ToModel()).ToList(),
                TaggedUsersId = entity.TaggedUsers?.Select(u => u.Id).ToList(),
                TaggedUsersUserName = entity.TaggedUsers?.Select(u => u.UserName).ToList(),
                Reactions = entity.Reactions?.Select(reaction => reaction.ToModel(UserPostReactionMappingsContext.Post)).ToList(),
                Comments = entity.Comments?.Select(comment => comment.ToModel(UserPostCommentMappingsContext.Post)).ToList(),
                Visibility = entity.Visibility,
                CreatedOn = entity.CreatedOn,
                UpdatedOn = entity.UpdatedOn,
                DeletedOn = entity.DeletedOn,
                CreatedBy = ShouldMapUser(context) ? entity.CreatedBy.ToModel(UserPostMappingsContext.Post) : null,
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
            return context == UserPostMappingsContext.Post || context == UserPostMappingsContext.Reaction;
        }
    }
}
