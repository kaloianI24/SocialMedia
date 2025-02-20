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

        public bool isAlreadyCreated (string tagName)
        {
            foreach (string tag in _context.Tags.Select(t => t.Name))
            {
                if (tag.ToLower() == tagName.ToLower())
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
