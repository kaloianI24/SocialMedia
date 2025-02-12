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

namespace SocialMedia.Service.Post
{
    public class SocialMediaPostService : ISocialMediaPostService
    {
        private readonly PostRepository postRepository;
        private readonly CloudResourceRepository cloudResourceRepository;
        private readonly TagRepository tagRepository;

        public SocialMediaPostService(PostRepository postRepository,
            CloudResourceRepository cloudResourceRepository,
            TagRepository tagRepository)
        {
            this.postRepository = postRepository;
            this.cloudResourceRepository = cloudResourceRepository;
            this.tagRepository = tagRepository;
        }

        public async Task<PostServiceModel> CreateAsync(PostServiceModel model)
        {
            SocialMediaPost post = model.ToEntity();

            //post.Attachments = post.Attachments.Select(async attachment =>
            //{
            //    return (await this.cloudResourceRepository.CreateAsync(attachment));
            //}).Select(a => a.Result).ToList();
            //post.Attachments = (await Task.WhenAll(post.Attachments.Select(async attachment =>
            //await this.cloudResourceRepository.CreateAsync(attachment)
            //))).ToList();

            var processedAttachments = new List<CloudResource>();

            foreach (var attachment in post.Attachments.Where(a => a != null))
            {
                var processedAttachment = await this.cloudResourceRepository.CreateAsync(attachment);
                processedAttachments.Add(processedAttachment);
            }
            post.Attachments = processedAttachments;

            var tags = new List<SocialMediaTag>();

            foreach (var tag in post.Tags)
            {
                if (!tagRepository.isAlreadyCreated(tag))
                {
                    await tagRepository.CreateAsync(tag);
                    tags.Add(tag);
                }
                else
                {
                    tags.Add(tagRepository.getTagByName(tag.Name));
                }
            }

            post.Tags = tags;            

            await postRepository.CreateAsync(post);

            return post.ToModel(UserPostMappingsContext.Post);
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
