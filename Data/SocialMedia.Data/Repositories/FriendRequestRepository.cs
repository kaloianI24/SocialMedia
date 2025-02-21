using Microsoft.AspNetCore.Http;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.Repositories
{
    public class FriendRequestRepository : MetadataBaseGenericRepository<FriendRequest>
    {
        public FriendRequestRepository(SocialMediaDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

    }
}
