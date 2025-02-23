// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialMedia.Areas.Identity.Data;
using SocialMedia.Data.Repositories;
using SocialMedia.Service.SocialMediaPost;

namespace SocialMedia.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<SocialMediaUser> _userManager;
        private readonly SignInManager<SocialMediaUser> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;
        private readonly PostRepository postRepository;
        private readonly ISocialMediaPostService socialMediaPostService;
        private readonly SocialMediaUserRepository socialMediaUserRepository;
        private readonly FriendRequestRepository friendRequestRepository;

        public DeletePersonalDataModel(
            UserManager<SocialMediaUser> userManager,
            SignInManager<SocialMediaUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,
            PostRepository postRepository,
            ISocialMediaPostService socialMediaPostService,
            SocialMediaUserRepository socialMediaUserRepository,
            FriendRequestRepository friendRequestRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            this.postRepository = postRepository;
            this.socialMediaPostService = socialMediaPostService;
            this.socialMediaUserRepository = socialMediaUserRepository;
            this.friendRequestRepository = friendRequestRepository;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }

            var posts =  postRepository.GetAll().Include(p => p.Attachments).Where(p => p.CreatedById == user.Id).ToList();
            var taggedPosts = await postRepository.UserTaggedPosts(user.Id);

            if (posts != null)
            {
                foreach(var post in posts)
                {
                    await socialMediaPostService.DeletePermanentlyAsync(post.Id);
                }               
            }

            if(taggedPosts.Count != 0)
            {
                foreach(var post in taggedPosts)
                {
                    await postRepository.RemoveTaggedUser(user, post);
                }
            }
            var requests = friendRequestRepository.GetAll().Where(fr => fr.CreatedById == user.Id || fr.ReceiverId == user.Id).ToList();
            foreach(var request in requests)
            {
                await friendRequestRepository.HardDeleteAsync(request);
            }

            var userDb = await socialMediaUserRepository.GetUserFullInformation(user.Id);
            var friends = userDb.Friends.ToList();
            foreach(var friend in friends)
            {
                userDb.Friends.Remove(friend);
                friend.Friends.Remove(userDb);
                await socialMediaUserRepository.UpdateAsync(friend);
            }

            var followers = userDb.Followers.ToList();
            foreach(var follower in followers)
            {
                userDb.Followers.Remove(follower);
                follower.Following.Remove(userDb);
                await socialMediaUserRepository.UpdateAsync(follower);
            }

            var followings = userDb.Following.ToList();

            foreach(var following in followings)
            {
                userDb.Following.Remove(following);
                following.Followers.Remove(userDb);
                await socialMediaUserRepository.UpdateAsync(following);
            }
            await socialMediaUserRepository.UpdateAsync(userDb);
            var result = await _userManager.DeleteAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }
    }
}
