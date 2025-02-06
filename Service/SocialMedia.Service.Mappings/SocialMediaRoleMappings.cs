using SocialMedia.Data.Models;
using SocialMedia.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Service.Mappings
{
    public static class SocialMediaRoleMappings
    {
        public static SocialMediaRole ToEntity(this SocialMediaRoleServiceModel model)
        {
            return new SocialMediaRole
            {
                //after admin get the visual effect change will be added
                Color = model.Color,
                Authority = model.Authority
            };
        }

        public static SocialMediaRoleServiceModel ToModel(this SocialMediaRole entity)
        {
            return new SocialMediaRoleServiceModel
            {
                Id = entity.Id,
                //after admin get the visual effect change will be added
                Color = entity.Color,
                Authority = entity.Authority,
            };
        }
    }
}
