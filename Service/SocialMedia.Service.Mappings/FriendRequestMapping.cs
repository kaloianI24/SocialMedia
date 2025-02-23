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
    public static class FriendRequestMapping
    {
    //    public static FriendRequest ToEntity(this FriendRequestServiceModel model)
    //    {
    //        return new FriendRequest
    //        {
    //            Receiver = model.ReceiverId.ToEntity(),
    //            Status = model.Status
    //        };
    //    }

        public static FriendRequestServiceModel ToModel(this FriendRequest entity)
        {
            return new FriendRequestServiceModel
            {
                Id = entity.Id,
                ReceiverId = entity.Receiver.Id,
                Status = entity.Status,
                CreatedById = entity.CreatedBy.Id                
            };
        }
    }
}
