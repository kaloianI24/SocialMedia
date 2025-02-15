using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SocialMedia.Areas.Identity.Pages.Account
{
    public class RegisterConfModel : PageModel
    {
        public string Email { get; set; }

        public void OnGet(string email)
        {
           
            Email = email;
        }
    }
}
