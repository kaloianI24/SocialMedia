using Microsoft.AspNetCore.Http;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.Repositories
{
    public class PostRepository : MetadataBaseGenericRepository<SocialMediaPost>
    {
        public PostRepository(SocialMediaDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
    }
}
