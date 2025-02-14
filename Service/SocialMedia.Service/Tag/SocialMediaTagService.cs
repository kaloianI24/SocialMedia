using SocialMedia.Data.Models;
using SocialMedia.Data.Repositories;
using SocialMedia.Service.Mappings;
using SocialMedia.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SocialMedia.Service.Mappings.SocialMediaPostMappings;

namespace SocialMedia.Service.Tag
{
    public class SocialMediaTagService : ISocialMediaTagService
    {
        private readonly TagRepository tagRepository;

        public SocialMediaTagService(TagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        public async Task<TagServiceModel> CreateAsync(TagServiceModel model)
        {
            return (await this.tagRepository.CreateAsync(model.ToEntity())).ToModel();
        }

        public Task<TagServiceModel> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TagServiceModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<TagServiceModel> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<SocialMediaTag> InternalCreateAsync(SocialMediaTag model)
        {
            throw new NotImplementedException();
        }

        public Task<TagServiceModel> UpdateAsync(TagServiceModel model)
        {
            throw new NotImplementedException();
        }
    }
}
