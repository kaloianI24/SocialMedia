using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using SocialMedia.Data.Models;
using SocialMedia.Service.Models;

namespace SocialMedia.Areas.Identity.Data;

// Add profile data for application users by adding properties to the SocialMediaUser class
public class SocialMediaUserServiceModel : IdentityUser
{
    [PersonalData]
    [Column(TypeName="nvarchar(100)")]
     public string FirstName { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; }

    public SocialMediaRoleServiceModel? Role { get; set; }

    public CloudResourceServiceModel? ProfilePicture { get; set; }

    public bool IsPrivate { get; set; }
    public List<SocialMediaUserBasicServiceModel>? Friends { get; set; } = new List<SocialMediaUserBasicServiceModel>();

    public List<SocialMediaUserBasicServiceModel>? Followers { get; set; } = new List<SocialMediaUserBasicServiceModel>();
    public List<SocialMediaUserBasicServiceModel>? Following { get; set; } = new List<SocialMediaUserBasicServiceModel>();

    public List<PostServiceModel>? Posts { get; set; } = new List<PostServiceModel>();
    public List<PostServiceModel>? SavedPosts { get; set; } = new List<PostServiceModel>();
    public List<PostServiceModel>? TaggedPosts { get; set; } = new List<PostServiceModel>();

    public List<SocialMediaUserServiceModel> TaggedUsers { get; set; } = new List<SocialMediaUserServiceModel>();
    public List<SocialMediaUserBasicServiceModel> BlockedUsers { get; set; } = new List<SocialMediaUserBasicServiceModel>();
}

