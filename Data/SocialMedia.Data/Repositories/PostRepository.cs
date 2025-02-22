using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Areas.Identity.Data;
using SocialMedia.Data.Models;
using System;

namespace SocialMedia.Data.Repositories
{
    public class PostRepository : MetadataBaseGenericRepository<SocialMediaPost>
    {
        public PostRepository(SocialMediaDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
        public async Task<SocialMediaPost> SoftDeletePost(SocialMediaPost post)
        {
            post.DeletedOn = DateTime.UtcNow;
            post.DeletedBy = await this.GetUser();
            _context.SaveChanges();
            return post;
        }

        public async Task<SocialMediaPost> Recover(SocialMediaPost post)
        {
            post.DeletedOn = null;
            post.DeletedBy = null;
            _context.SaveChanges();
            return post;
        }

        public async Task<SocialMediaPost> RemoveTaggedUser(SocialMediaUser user, SocialMediaPost post)
        {
            post.TaggedUsers.Remove(user);
            _context.SaveChanges();
            return post;
        }

        public async Task<List<SocialMediaPost>> UserTaggedPosts(string userId)
        {
            return await _context.Posts
                .Include(p => p.TaggedUsers)
                .Where(p => p.TaggedUsers.Any(u => u.Id == userId))
                .Include(p => p.CreatedBy)
                .ToListAsync();
        }
        public async Task HardDeleteAsync(SocialMediaPost post)
        {
            _context.Remove(post);
            await _context.SaveChangesAsync();
        }

    }
}
