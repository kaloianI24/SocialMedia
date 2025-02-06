using Microsoft.AspNetCore.Http;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.Repositories
{
    public class TagRepository : MetadataBaseGenericRepository<SocialMediaTag>
    {
        public TagRepository(SocialMediaDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
    }
}
