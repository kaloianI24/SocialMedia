using SocialMedia.Data.Models;
using SocialMedia.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SocialMedia.Service.Mappings.SocialMediaPostMappings;

namespace SocialMedia.Service.Mappings
{
    public static class SocialMediaTagMappings
    {
        public static SocialMediaTag ToEntity(this TagServiceModel model)
        {
            return new SocialMediaTag
            {
                Name = model.Name
            };
        }

        public static TagServiceModel ToModel(this SocialMediaTag entity, UserPostMappingsContext context)
        {
            return new TagServiceModel
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedOn = entity.CreatedOn,
                UpdatedOn = entity.UpdatedOn,
                DeletedOn = entity.DeletedOn,
                CreatedBy = ShouldMapUser(context) ? entity.CreatedBy.ToModel(UserPostMappingsContext.Tag) : null,
                UpdatedBy = ShouldMapUser(context) ? entity.UpdatedBy?.ToModel(UserPostMappingsContext.Tag) : null,
                DeletedBy = ShouldMapUser(context) ? entity.DeletedBy?.ToModel(UserPostMappingsContext.Tag) : null,
            };
        }
    }
}