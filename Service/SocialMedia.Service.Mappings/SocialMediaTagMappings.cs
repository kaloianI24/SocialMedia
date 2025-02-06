using SocialMedia.Data.Models;
using SocialMedia.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static TagServiceModel ToModel(this SocialMediaTag entity)
        {
            return new TagServiceModel
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedOn = entity.CreatedOn,
                UpdatedOn = entity.UpdatedOn,
                DeletedOn = entity.DeletedOn,
                CreatedBy = entity.CreatedBy.ToModel(),
                UpdatedBy = entity.UpdatedBy?.ToModel(),
                DeletedBy = entity.DeletedBy?.ToModel()
            };
        }
    }
}