using Microsoft.AspNetCore.Identity;
using SocialMedia.Areas.Identity.Data;

public class SeedRolesAndUsers
{
    public static async Task EnsureAdminRoleAndUserAsync(UserManager<SocialMediaUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var role = await roleManager.FindByNameAsync("Admin");
        if (role == null)
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        var adminUser = await userManager.FindByNameAsync("admin");
        if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}
