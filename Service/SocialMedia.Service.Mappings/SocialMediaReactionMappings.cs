using SocialMedia.Data.Models;
using SocialMedia.Service.Models;
using SocialMedia.Service.Mappings;
using static SocialMedia.Service.Mappings.SocialMediaPostMappings;

namespace SocialMedia.Service.Mappings
{
    public static class ReactionMappings
    {
        public static SocialMediaReaction ToEntity(this SocialMediaReactionServiceModel model)
        {
            return new SocialMediaReaction
            {
                Label = model.Label,
                Emote = model.Emote.ToEntity()
            };
        }
        public static SocialMediaReactionServiceModel ToModel(this SocialMediaReaction entity, UserPostMappingsContext context)
        {
            return new SocialMediaReactionServiceModel
            {
                Id = entity.Id,
                Label = entity.Label,
                Emote = entity.Emote?.ToModel(),
                CreatedOn = entity.CreatedOn,
                UpdatedOn = entity.UpdatedOn,
                DeletedOn = entity.DeletedOn,
                CreatedBy = ShouldMapUser(context) ? entity.CreatedBy.ToModel(UserPostMappingsContext.Post) : null,
                UpdatedBy = ShouldMapUser(context) ? entity.CreatedBy.ToModel(UserPostMappingsContext.Post) : null,
                DeletedBy = ShouldMapUser(context) ? entity.CreatedBy.ToModel(UserPostMappingsContext.Post) : null,
            };
        }
    }
}