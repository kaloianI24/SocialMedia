using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using SocialMedia.Data.Models;

namespace SocialMedia.Areas.Identity.Data;

public class SocialMediaUser : IdentityUser
{
    [PersonalData]
    [Column(TypeName="nvarchar(100)")]
     public string FirstName { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; }

    public bool IsPrivate { get; set; }
    public SocialMediaRole? Role { get; set; }

    public CloudResource? ProfilePicture { get; set; }

    public List<SocialMediaUser>? Friends { get; set; } = new List<SocialMediaUser>();

    public List<SocialMediaUser>? Following { get; set; } = new List<SocialMediaUser>();
    public List<SocialMediaUser>? Followers { get; set; } = new List<SocialMediaUser>();

    public List<SocialMediaPost>? Posts { get; set; } = new List<SocialMediaPost>();
    public List<SocialMediaPost>? TaggedPosts { get; set; } = new List<SocialMediaPost>();
    public List<SocialMediaPost>? SavedPosts { get; set; } = new List<SocialMediaPost>();

    public List<FriendRequest>? SentFriendRequests { get; set; } = new List<FriendRequest>();

    public List<FriendRequest>? ReceivedFriendRequests { get; set; } = new List<FriendRequest>();

    public List<SocialMediaUser>? BlockedUsers { get; set; } = new List<SocialMediaUser>();
    public List<ChatMessage> MessagesSent { get; set; }
    public List<ChatMessage> MessagesReceived { get; set; }
    public List<Notification> Notifications { get; set; }
    public bool AcceptedTerms { get; set; }
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";

    public bool IsDeleted { get; set; }
}

