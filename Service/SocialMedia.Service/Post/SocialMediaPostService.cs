using SocialMedia.Data.Models;
using SocialMedia.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Service.Post
{
    internal class SocialMediaPostService : ISocialMediaPostService
    {
        public Task<PostServiceModel> CreateAsync(PostServiceModel model)
        {
            throw new NotImplementedException();
        }

        public Task<PostServiceModel> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PostServiceModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<PostServiceModel> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PostServiceModel> InternalCreateAsync(PostServiceModel model)
        {
            throw new NotImplementedException();
        }

        public Task<SocialMediaPost> InternalCreateAsync(SocialMediaPost model)
        {
            throw new NotImplementedException();
        }

        public Task<PostServiceModel> UpdateAsync(PostServiceModel model)
        {
            throw new NotImplementedException();
        }
    }
}
