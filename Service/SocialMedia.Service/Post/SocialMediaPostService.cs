using Microsoft.EntityFrameworkCore;
using SocialMedia.Data.Models;
using SocialMedia.Data.Repositories;
using SocialMedia.Service.Mappings;
using SocialMedia.Service.Models;
using static SocialMedia.Service.Mappings.SocialMediaPostMappings;

namespace SocialMedia.Service.Post
{
    public class SocialMediaPostService : ISocialMediaPostService
    {
        private readonly PostRepository postRepository;
        private readonly CloudResourceRepository cloudResourceRepository;
        private readonly TagRepository tagRepository;
        private readonly SocialMediaUserRepository userRepository;

        public SocialMediaPostService(PostRepository postRepository,
            CloudResourceRepository cloudResourceRepository,
            TagRepository tagRepository,
            SocialMediaUserRepository userRepository)
        {
            this.postRepository = postRepository;
            this.cloudResourceRepository = cloudResourceRepository;
            this.tagRepository = tagRepository;
            this.userRepository = userRepository;
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

            if(model.TaggedUsersId != null)
            {
                post.TaggedUsers = await userRepository.GetUsersByIdsAsync(model.TaggedUsersId);
            }
            
            await postRepository.CreateAsync(post);

            return post.ToModel(UserPostMappingsContext.Post);
        }

        public IQueryable<PostServiceModel> GetAllTaggedPosts(string id)
        {
            return postRepository.GetAll().Where(p => p.TaggedUsers.Any(u => u.Id == id) && p.DeletedBy == null)
                .Include(p => p.Attachments)
                .Include(p => p.CreatedBy)
                .ThenInclude(c => c.ProfilePicture)
                .Include(p => p.Tags)
                .Select(p => p.ToModel(UserPostMappingsContext.Post));
        }
        public async Task<PostServiceModel> DeleteAsync(string id)
        {
            var targetPost = await postRepository.GetAll().FirstOrDefaultAsync(p => p.Id == id);
            await postRepository.SoftDeletePost(targetPost);
            return targetPost.ToModel(UserPostMappingsContext.Post);
        }

        public async Task<PostServiceModel> RecoverAsync(string id)
        {
            var targetPost = await postRepository.GetAll().FirstOrDefaultAsync(p => p.Id == id);
            await postRepository.Recover(targetPost);
            return targetPost.ToModel(UserPostMappingsContext.Post);
        }

        public async Task<PostServiceModel> RemoveTaggedUser(string userId, string postId)
        {
            var targetPost = await postRepository.GetAll()
                .Include(p => p.TaggedUsers)
                .ThenInclude(u => u.ProfilePicture)
                .Include(p => p.Tags)
                .Include(p => p.Attachments)
                .Include(p => p.CreatedBy)
                .ThenInclude(u => u.ProfilePicture)
                .FirstOrDefaultAsync(p => p.Id == postId);

            var targetUser = targetPost.TaggedUsers.FirstOrDefault(u => u.Id == userId);
            if (targetUser != null)
            {
                await postRepository.RemoveTaggedUser(targetUser, targetPost);
            }
            return targetPost.ToModel(UserPostMappingsContext.Post);
        }
        public IQueryable<PostServiceModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<PostServiceModel> GetByIdAsync(string id)
        {
            var targetPost = await postRepository.GetAll().FirstOrDefaultAsync(p => p.Id == id);
            var targetPostServiceModel = targetPost.ToModel(UserPostMappingsContext.Post);
            return targetPostServiceModel;
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
