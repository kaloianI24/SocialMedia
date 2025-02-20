using Microsoft.EntityFrameworkCore;
using SocialMedia.Areas.Identity.Data;
using SocialMedia.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Data.Repositories
{
    public class SocialMediaUserRepository
    {
        private readonly SocialMediaDbContext _context;

        public SocialMediaUserRepository(SocialMediaDbContext context)
        {
            _context = context;
        }

        public async Task<List<SocialMediaUser>> GetUsersByIdsAsync(List<string> ids)
        {
            var users = new List<SocialMediaUser>();
            foreach(string id in ids)
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == id);
                users.Add(user);
            }
            return users;
        }

        public async Task<SocialMediaUser> GetUserById(string id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public async Task<SocialMediaUser> GetUserFullInformation(string id)
        {
            return  _context.Users
                .Include(u => u.Posts)
                    .ThenInclude(p => p.Attachments)
                .FirstOrDefault(u => u.Id == id);
        }
    }
}
