using Microsoft.AspNetCore.Mvc;
using SocialMedia.Areas.Identity.Data;
using SocialMedia.Data.Models;
using SocialMedia.Service.Models;
using SocialMedia.Web.Models.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Service.SocialMediaPost
{
    public interface ISocialMediaPostService : IGenericService<Data.Models.SocialMediaPost, PostServiceModel>
    {
        public IQueryable<PostServiceModel> GetAllTaggedPosts(SocialMediaUser user, bool isOwner, SocialMediaUser currentUser);

        public Task<PostServiceModel> RecoverAsync(string id);

        public Task<PostServiceModel> RemoveTaggedUser(string userId, string postId);
        public Task<bool> DeletePermanentlyAsync(string id);
        public Task<PostServiceModel> UpdateAsync(UpdatePostWebModel model);
    }

}