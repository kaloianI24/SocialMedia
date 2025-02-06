using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using SocialMedia.Data.Models;

namespace SocialMedia.Areas.Identity.Data;

// Add profile data for application users by adding properties to the SocialMediaUser class
public class SocialMediaUser : IdentityUser
{
    [PersonalData]
    [Column(TypeName="nvarchar(100)")]
     public string FirstName { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; }

    public SocialMediaRole? Role { get; set; }

    public CloudResource? ProfilePicture { get; set; }

    public List<SocialMediaUser>? Friends { get; set; } = new List<SocialMediaUser>();

    public List<SocialMediaUser>? Followers { get; set; } = new List<SocialMediaUser>();

    public List<Post>? Posts { get; set; } = new List<Post>();
    public List<Post>? TaggedPosts { get; set; } = new List<Post>();
}

