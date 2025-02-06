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
        public static SocialMediaPost ToEntity(this PostServiceModel model)
        {
            return new SocialMediaPost
            {
                Description = model.Description,
                Attachments = model.Attachments,
                Tags = model.Tags,
                TaggedUsers = model.TaggedUsers,
            };
        }

        public static PostServiceModel ToEntity(this SocialMediaPost entity)
        {
            return new PostServiceModel
            {
                Id = entity.Id,
                Description = entity.Description,
                Attachments = entity.Attachments,
                Tags = entity.Tags,
                TaggedUsers = entity.TaggedUsers,
                CreatedOn = entity.CreatedOn,
                UpdatedOn = entity.UpdatedOn,
                DeletedOn = entity.DeletedOn,
                CreatedBy = entity.CreatedBy.ToModel(),
                UpdatedBy = entity.UpdatedBy?.ToModel(),
                DeletedBy = entity.DeletedBy?.ToModel(),
            };
        }
    }
}
