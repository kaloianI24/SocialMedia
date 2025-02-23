using SocialMedia.Data.Models;
using SocialMedia.Service.Models;
using static SocialMedia.Service.Mappings.SocialMediaPostMappings;

namespace SocialMedia.Service.Mappings
{
    public enum UserPostReactionMappingsContext
    {
        Post,
        Reaction,
        User
    }
    public static class UserPostReactionMappings
    {
        public static UserPostReactionServiceModel ToModel(this UserPostReaction entity, UserPostReactionMappingsContext context)
        {
            return new UserPostReactionServiceModel
            {
                Id = entity.Id,
                Reaction = ShouldMapReaction(context) ? entity.Reaction?.ToModel(UserPostMappingsContext.Post) : null,
                Post = ShouldMapPost(context) ? entity.Post?.ToModel(UserPostMappingsContext.Reaction) : null,
                User = ShouldMapUser(context) ? entity.User?.ToModel(UserPostMappingsContext.Post) : null
            };
        }

        private static bool ShouldMapReaction(UserPostReactionMappingsContext context)
        {
            return context == UserPostReactionMappingsContext.Post || context == UserPostReactionMappingsContext.User;
        }

        private static bool ShouldMapPost(UserPostReactionMappingsContext context)
        {
            return context == UserPostReactionMappingsContext.Reaction || context == UserPostReactionMappingsContext.User;
        }

        private static bool ShouldMapUser(UserPostReactionMappingsContext context)
        {
            return context == UserPostReactionMappingsContext.Post || context == UserPostReactionMappingsContext.Reaction;
        }
    }
}
