using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.Models;
using SocialMedia.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Service.SocialMediaPost
{
    public interface ISocialMediaPostService : IGenericService<Data.Models.SocialMediaPost, PostServiceModel>
    {
        public IQueryable<PostServiceModel> GetAllTaggedPosts(string id);

        public Task<PostServiceModel> RecoverAsync(string id);

        public Task<PostServiceModel> RemoveTaggedUser(string userId, string postId);
    }

}