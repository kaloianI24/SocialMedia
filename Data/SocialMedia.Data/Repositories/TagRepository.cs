using Microsoft.AspNetCore.Http;
using SocialMedia.Data.Models;
using System.Diagnostics;

namespace SocialMedia.Data.Repositories
{
    public class TagRepository : BaseGenericRepository<SocialMediaTag>
    {
        public TagRepository(SocialMediaDbContext context) : base(context)
        {
        }

        public bool isAlreadyCreated (SocialMediaTag socialMediaTag)
        {
            foreach (SocialMediaTag tag in _context.Tags)
            {
                if (tag.Name.ToLower() == socialMediaTag.Name.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        public SocialMediaTag getTagByName (string name)
        {
            foreach (SocialMediaTag tag in _context.Tags)
            {
                if (tag.Name.ToLower() == name.ToLower())
                {
                    return tag;
                }
            }
            return null;
        }
    }
}
