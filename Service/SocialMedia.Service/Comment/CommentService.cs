using Microsoft.EntityFrameworkCore;
using SocialMedia.Data.Models;
using SocialMedia.Data.Repositories;
using SocialMedia.Service.Mappings;
using SocialMedia.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Service.Comment
{
    public class CommentService : ICommentService
    {
        private readonly CommentRepository commentRepository;
        private readonly PostRepository postRepository;
        private readonly ReactionRepository reactionRepository;
        private readonly SocialMediaUserRepository userRepository;

        public CommentService(CommentRepository commentRepository, PostRepository postRepository, ReactionRepository reactionRepository, SocialMediaUserRepository userRepository)
        {
            this.commentRepository = commentRepository;
            this.postRepository = postRepository;
            this.reactionRepository = reactionRepository;
            this.userRepository = userRepository;
        }

        public async Task<SocialMediaCommentServiceModel> CreateAsync(SocialMediaCommentServiceModel model)
        {
            SocialMediaComment entity = model.ToEntity();

            return (await this.commentRepository.CreateAsync(entity)).ToModel(CommentMappingsContext.Reaction);
        }

        public Task<SocialMediaCommentServiceModel> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<SocialMediaCommentServiceModel> GetAll()
        {
            return this.InternalGetAll().Select(comment => comment.ToModel(CommentMappingsContext.User));
        }

        public IQueryable<SocialMediaCommentServiceModel> GetAllByParentId(string parentId)
        {
            return this.InternalGetAll().Where(comment => comment.Id == parentId).Select(comment => comment.ToModel(CommentMappingsContext.Parent));
        }

        public Task<SocialMediaCommentServiceModel> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<SocialMediaComment> InternalCreateAsync(SocialMediaComment model)
        {
            throw new NotImplementedException();
        }

        public Task<SocialMediaCommentServiceModel> UpdateAsync(SocialMediaCommentServiceModel model)
        {
            throw new NotImplementedException();
        }

        private IQueryable<SocialMediaComment> InternalGetAll()
        {
            return commentRepository.GetAll()
                .Include(c => c.Attachments)
                .Include(t => t.Reactions)
                .Include(c => c.Replies)
                .Include(t => t.CreatedBy)
                .Include(t => t.UpdatedBy)
                .Include(t => t.DeletedBy);
        } 
    }
}
