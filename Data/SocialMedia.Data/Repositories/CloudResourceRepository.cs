using SocialMedia.Data.Models;

namespace SocialMedia.Data.Repositories
{
    public class CloudResourceRepository : BaseGenericRepository<CloudResource>
    {
        public CloudResourceRepository(SocialMediaDbContext context) : base(context)
        {
        }
    }
}
