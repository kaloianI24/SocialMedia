using SocialMedia.Data.Models;

namespace SocialMedia.Data.Repositories
{
    //Might have to change that into PostRepository.cs
    public class CloudResourceRepository : BaseGenericRepository<CloudResource>
    {
        public CloudResourceRepository(SocialMediaDbContext context) : base(context)
        {
        }
    }
}
