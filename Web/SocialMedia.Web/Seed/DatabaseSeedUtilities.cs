using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Areas.Identity.Data;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Seed
{
    public static class DatabaseSeedUtilities
    {
        public static void UseDatabaseSeed(this WebApplication app)
        {
            SeedRoles(app);
        }

        public static void SeedRoles(WebApplication app)
        {
            using (var serviceScope = app.Services.CreateScope())
            {
                using (var socialMediaDbContext = serviceScope.ServiceProvider.GetRequiredService<SocialMediaDbContext>())
                {
                    socialMediaDbContext.Database.Migrate();

                    if (socialMediaDbContext.Roles.Count() == 0)
                    {
                        IdentityRole adminRole = new IdentityRole();
                        adminRole.Name = "Administrator";
                        adminRole.NormalizedName = adminRole.Name.ToUpper();

                        IdentityRole userRole = new IdentityRole();
                        userRole.Name = "User";
                        userRole.NormalizedName = userRole.Name.ToUpper();

                        socialMediaDbContext.Add(adminRole);
                        socialMediaDbContext.Add(userRole);

                        socialMediaDbContext.SaveChanges();
                    }
                }
            }
        }
    }
}
