using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Areas.Identity.Data;
using SocialMedia.Data.Models;
using SocialMedia.Data.Repositories;
using SocialMedia.Service.Cloud;
using SocialMedia.Service.Mappings;
using SocialMedia.Service.Models;
using SocialMedia.Web.Models.Post;
using static SocialMedia.Service.Mappings.SocialMediaPostMappings;

namespace SocialMedia.Service.SocialMediaPost
{
    public class SocialMediaPostService : ISocialMediaPostService
    {
        private readonly PostRepository postRepository;
        private readonly CloudResourceRepository cloudResourceRepository;
        private readonly TagRepository tagRepository;
        private readonly SocialMediaUserRepository userRepository;
        private readonly ICloudinaryService cloudinaryService;

        public SocialMediaPostService(PostRepository postRepository,
            CloudResourceRepository cloudResourceRepository,
            TagRepository tagRepository,
            SocialMediaUserRepository userRepository,
            ICloudinaryService cloudinaryService)
        {
            this.postRepository = postRepository;
            this.cloudResourceRepository = cloudResourceRepository;
            this.tagRepository = tagRepository;
            this.userRepository = userRepository;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<PostServiceModel> CreateAsync(PostServiceModel model)
        {
            Data.Models.SocialMediaPost post = model.ToEntity();

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
                if(tag.Name != null && !tag.Name.Equals(""))
                {
                    if (!tagRepository.isAlreadyCreated(tag.Name))
                    {
                        await tagRepository.CreateAsync(tag);
                        tags.Add(tag);
                    }
                    else
                    {
                        tags.Add(tagRepository.getTagByName(tag.Name));
                    }
                }                
            }

            post.Tags = tags;            

            if(model.TaggedUsersId != null)
            {
                post.TaggedUsers = await userRepository.GetUsersByIdsAsync(model.TaggedUsersId);
            }

            post.Visibility = model.Visibility;
            await postRepository.CreateAsync(post);

            return post.ToModel(UserPostMappingsContext.Post);
        }

        //public IQueryable<PostServiceModel> GetAllTaggedPosts(SocialMediaUser user, bool isOwner, SocialMediaUser currenUser)
        //{
        //    if(isOwner)
        //    {
        //        return postRepository.GetAll()
        //        .Where(p => p.TaggedUsers.Any(u => u.Id == user.Id) && p.DeletedBy == null)
        //        .Include(p => p.Attachments)
        //        .Include(p => p.CreatedBy)
        //        .ThenInclude(c => c.ProfilePicture)
        //        .Include(p => p.Tags)
        //        .Select(p => p.ToModel(UserPostMappingsContext.Post));
        //    }

        //    return postRepository.GetAll()
        //        .Where(p => p.TaggedUsers.Any(u => u.Id == user.Id)
        //        && p.DeletedBy == null &&
        //        (
        //            !p.CreatedBy.IsPrivate ||
        //            (p.CreatedBy.IsPrivate && currenUser.Friends.Contains(p.CreatedBy)
        //            ||p.CreatedBy.Id == currenUser.Id)
        //        ))
        //       .Include(p => p.Attachments)
        //       .Include(p => p.CreatedBy)
        //       .ThenInclude(c => c.ProfilePicture)
        //       .Include(p => p.Tags)
        //       .Select(p => p.ToModel(UserPostMappingsContext.Post));

        //}
        public async Task<PostServiceModel> DeleteAsync(string id)
        {
            var targetPost = await postRepository.GetAll().FirstOrDefaultAsync(p => p.Id == id);
            await postRepository.SoftDeletePost(targetPost);
            return targetPost.ToModel(UserPostMappingsContext.Post);
        }

        public async Task<bool> DeletePermanentlyAsync(string id)
        {
            var targetPost = await postRepository.GetAll().Include(p => p.Attachments).FirstOrDefaultAsync(p => p.Id == id);
            var attachments = targetPost.Attachments.ToList();
            foreach(var attachment in attachments)
            {
                await cloudinaryService.DeleteFileAsync(attachment.CloudUrl);
                await cloudResourceRepository.DeleteAsync(attachment);
            }
            await postRepository.HardDeleteAsync(targetPost);
            return true;
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
            var targetPost = await postRepository.GetAll()
                .Include(p => p.TaggedUsers)
                    .ThenInclude(u => u.ProfilePicture)
                .Include(p => p.Tags)
                .Include(p => p.Attachments)
                .Include(p => p.CreatedBy)
                    .ThenInclude(u => u.ProfilePicture)
                .FirstOrDefaultAsync(p => p.Id == id);
            var targetPostServiceModel = targetPost.ToModel(UserPostMappingsContext.Post);
            return targetPostServiceModel;
        }

        public async Task<PostServiceModel> UpdateAsync(UpdatePostWebModel model)
        {
            var targetPost = await postRepository.GetAll()
                .Include(p => p.TaggedUsers)
                    .ThenInclude(u => u.ProfilePicture)
                .Include(p => p.Tags)
                .Include(p => p.Attachments)
                .Include(p => p.CreatedBy)
                    .ThenInclude(u => u.ProfilePicture)
                .FirstOrDefaultAsync(p => p.Id == model.Id);

            var targetPostAttachments = targetPost.Attachments.ToList();

            var tags = new List<SocialMediaTag>();

            if(model.Tags != null)
            {
                foreach (var tag in model.Tags.Split(","))
                {
                    if (tag != null && !tag.Equals(""))
                    {
                        if (!tagRepository.isAlreadyCreated(tag))
                        {
                            var tagEntity = (new TagServiceModel { Name = tag }).ToEntity();
                            await tagRepository.CreateAsync(tagEntity);
                            tags.Add(tagEntity);
                        }
                        else
                        {
                            tags.Add(tagRepository.getTagByName(tag));
                        }
                    }
                }
            }
            

            foreach (var attachment in targetPostAttachments)
            {
                if(model.RemovedAttachmentIds != null)
                {
                    foreach (var attachmentId in model.RemovedAttachmentIds.Split(",").ToList())
                    {
                        if (attachment.Id == attachmentId)
                        {
                            targetPost.Attachments.Remove(attachment);
                            await cloudinaryService.DeleteFileAsync(attachment.CloudUrl);
                            await cloudResourceRepository.DeleteAsync(attachment);
                        }
                    }
                }
                
            }
           
            var firstId = model.TaggedUsersId.First();
            if (firstId != null)
            {
                var usersIds = model.TaggedUsersId.First().Split(",");
                var tasks = usersIds.Select(async id => await userRepository.GetUserById(id)).ToList();
                var taggedUsers = await Task.WhenAll(tasks);
                targetPost.TaggedUsers = taggedUsers.ToList();
            }
            else
            {
                targetPost.TaggedUsers = null;
            }

            targetPost.Description = model.Description;
            targetPost.Tags = tags;
            targetPost.Visibility = model.Visibility ?? "friends";
            
            await postRepository.UpdateAsync(targetPost);
            return targetPost.ToModel(UserPostMappingsContext.Post);
        }

        public async Task<PostServiceModel> SavePost(string postId, SocialMediaUser user)
        {
            bool userHasAlreadySavedPost = user.SavedPosts.Select(p => p.Id).Contains(postId);
            if(userHasAlreadySavedPost)
            {
                throw new Exception("You have already saved the post!");
            }

            else
            {
                var post = postRepository.GetAll().Include(p => p.Attachments).FirstOrDefault(p => p.Id == postId);
                user.SavedPosts.Add(post);
                await userRepository.UpdateAsync(user);
                await postRepository.UpdateAsync(post);
                return post.ToModel(UserPostMappingsContext.Post);
            }
        }

        public async Task<PostServiceModel> UnsavePost(string postId, SocialMediaUser user)
        {
            bool postIsSaved = user.SavedPosts.Select(p => p.Id).Contains(postId);
            if (postIsSaved)
            {
                var post = postRepository.GetAll().Include(p => p.Attachments).FirstOrDefault(p => p.Id == postId);
                user.SavedPosts.Remove(post);
                await userRepository.UpdateAsync(user);
                await postRepository.UpdateAsync(post);
                return post.ToModel(UserPostMappingsContext.Post);
            }

            else
            {
                throw new Exception("You did not save the post!");
            }
        }
        public Task<PostServiceModel> InternalCreateAsync(PostServiceModel model)
        {
            throw new NotImplementedException();
        }

        public Task<Data.Models.SocialMediaPost> InternalCreateAsync(Data.Models.SocialMediaPost model)
        {
            throw new NotImplementedException();
        }

        public Task<PostServiceModel> UpdateAsync(PostServiceModel model)
        {
            throw new NotImplementedException();
        }
    }
}
