// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SocialMedia.Areas.Identity.Data;
using SocialMedia.Service.Cloud;
using SocialMedia.Service.Mappings;
using SocialMedia.Service.Models;

namespace SocialMedia.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private const string AdministratorRole = "Administrator";

        private const string UserRole = "User";

        private readonly SignInManager<SocialMediaUser> _signInManager;
        private readonly UserManager<SocialMediaUser> _userManager;
        private readonly IUserStore<SocialMediaUser> _userStore;
        private readonly IUserEmailStore<SocialMediaUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ICloudinaryService _cloudinaryService;

        public RegisterModel(
            UserManager<SocialMediaUser> userManager,
            IUserStore<SocialMediaUser> userStore,
            SignInManager<SocialMediaUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ICloudinaryService cloudinaryService)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _cloudinaryService = cloudinaryService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [DataType(DataType.Upload)]
            public IFormFile ProfilePicture { get; set; }

            [Required]
            public bool IsPrivate { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public bool AgreeTerms { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.UserName = Input.Email; 
                user.Email = Input.Email;
                user.AcceptedTerms = Input.AgreeTerms; 

                if (Input.ProfilePicture is not null)
                {
                    string profilePhotoUrl = await UploadPhoto(Input.ProfilePicture);
                    user.ProfilePicture = new CloudResourceServiceModel { CloudUrl = profilePhotoUrl }.ToEntity();
                }
                else
                {
                    user.ProfilePicture = new CloudResourceServiceModel { CloudUrl = "https://res.cloudinary.com/socialmedia-itcareer/image/upload/v1739556301/User_pfp_simple_qggx4n.jpg" }.ToEntity();
                }

                await _userStore.SetUserNameAsync(user, Input.Username, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                try
                {
                    var result = await _userManager.CreateAsync(user, Input.Password);

                    if (result.Succeeded)
                    {
                        if (_userManager.Users.Count() == 1)
                        {
                            await _userManager.AddToRoleAsync(user, AdministratorRole);
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(user, UserRole);
                        }

                        _logger.LogInformation("User created a new account with password.");

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/RegisterConf",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("/Account/RegisterConf", new { area = "Identity", email = Input.Email, returnUrl = returnUrl });
                        }
                        else
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Database error: {Message}", ex.InnerException?.Message ?? ex.Message);
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the user. Please try again later.");
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private SocialMediaUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<SocialMediaUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(SocialMediaUser)}'. " +
                    $"Ensure that '{nameof(SocialMediaUser)}' is not an abstract class and has a parameterless constructor.");
            }
        }

        private IUserEmailStore<SocialMediaUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<SocialMediaUser>)_userStore;
        }

        private async Task<string> UploadPhoto(IFormFile photo)
        {
            var uploadResponse = await this._cloudinaryService.UploadFile(photo);

            if (uploadResponse == null)
            {
                return null;
            }

            return uploadResponse["url"].ToString();
        }
    }
}
