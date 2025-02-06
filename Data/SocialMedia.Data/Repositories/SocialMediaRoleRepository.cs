using Microsoft.AspNetCore.Http;
using SocialMedia.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Data.Repositories
{
    public class SocialMediaRoleRepository : BaseGenericRepository<SocialMediaRole>
    {
        public SocialMediaRoleRepository(SocialMediaDbContext context) : base(context)
        {
        }
    }
}
