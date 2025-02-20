using SocialMedia.Data.Models;
using SocialMedia.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Service.Mappings
{
    public static class CloudResourceMappings
    {
        public static CloudResource ToEntity(this CloudResourceServiceModel model)
        {
            return new CloudResource
            {
                CloudUrl = model.CloudUrl,
            };
        }

        public static CloudResourceServiceModel ToModel(this CloudResource entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new CloudResourceServiceModel
            {
                CloudUrl = entity.CloudUrl,
                Id = entity.Id
            };
        }
    }
}
