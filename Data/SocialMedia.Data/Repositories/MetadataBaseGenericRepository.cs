using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Areas.Identity.Data;
using SocialMedia.Data.Models;
using System.Security.Claims;

namespace SocialMedia.Data.Repositories
{
    public abstract class MetadataBaseGenericRepository<TEntity> : BaseGenericRepository<TEntity> where TEntity : MetadataBaseEntity
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected MetadataBaseGenericRepository(SocialMediaDbContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        public override async Task<TEntity> CreateAsync(TEntity entity)
        {
            //Utc -> most common timezone
            entity.CreatedOn = DateTime.UtcNow;
            entity.CreatedBy = await this.GetUser();
            return await base.CreateAsync(entity);
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity)
        {
            entity.UpdatedOn = DateTime.UtcNow;
            entity.UpdatedBy = await this.GetUser();
            return await base.UpdateAsync(entity);
        }

        public override async Task<TEntity> DeleteAsync(TEntity entity)
        {
            entity.DeletedOn = DateTime.UtcNow;
            entity.DeletedBy = await this.GetUser();
            return await base.DeleteAsync(entity);
        }

        private async Task<SocialMediaUser> GetUser()
        {
            string? userId = this._httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await this._context.Users
                .Include(u => u.ProfilePicture)
                .SingleOrDefaultAsync(user => user.Id == userId);
        }
    }
}
