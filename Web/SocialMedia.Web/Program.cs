using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Areas.Identity.Data;
using SocialMedia.Data;
using SocialMedia.Data.Repositories;
using SocialMedia.Seed;
using SocialMedia.Service.Cloud;
using SocialMedia.Service.SocialMediaPost;
using SocialMedia;
using System;
using SocialMedia.Service;
using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Service.Reaction;
using SocialMedia.Service.Friends;
using SocialMedia.Service.Comment;
using SocialMedia.Service.Encryption;
using SocialMedia.Service.Hub;
using Microsoft.AspNetCore.SignalR;

namespace SocialMedia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Fetch the MySQL connection string from appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("SocialMediaDbContextConnection")
                ?? throw new InvalidOperationException("Connection string 'SocialMediaDbContextConnection' not found.");

            builder.Services.AddDbContext<SocialMediaDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // Configure Identity
            builder.Services.AddDefaultIdentity<SocialMediaUser>(options =>
                options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<SocialMediaDbContext>();

            // Register Email Sender
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            // Register other services
            builder.Services.AddTransient<PostRepository>();
            builder.Services.AddTransient<CloudResourceRepository>();
            builder.Services.AddTransient<TagRepository>();
            builder.Services.AddTransient<SocialMediaUserRepository>();
            builder.Services.AddTransient<ReactionRepository>();
            builder.Services.AddTransient<FriendRequestRepository>();
            builder.Services.AddTransient<CommentRepository>();
            builder.Services.AddTransient<ChatMessageRepository>();

            builder.Services.AddTransient<ICloudinaryService, CloudinaryService>();
            builder.Services.AddTransient<IReactionService, ReactionService>();
            builder.Services.AddTransient<ISocialMediaPostService, SocialMediaPostService>();
            builder.Services.AddTransient<IFriendRequestService, FriendRequestService>();
            builder.Services.AddTransient<ICommentService, CommentService>();
            builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            builder.Services.AddSingleton<IEncryptionService, EncryptionService>(provider =>
            {
                var encryptionKey = builder.Configuration["EncryptionKey"] ??
                    throw new InvalidOperationException("EncryptionKey not configured");
                return new EncryptionService(encryptionKey);
            });

            builder.Services.AddSignalR()
                .AddHubOptions<ChatHub>(options =>
                {
                    options.EnableDetailedErrors = true;
                });

            // Add services to the container
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddRouting();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseDatabaseSeed();
            app.UseHttpsRedirection();
            app.UseStaticFiles();  // Ensures static files are served properly
            app.UseRouting();
            app.UseAuthentication();  // Ensure Identity authentication is used
            app.UseAuthorization();

            // Map static assets and routes
            app.MapControllerRoute(
                name: "Administration",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Post}/{action=MyPage}/{userId?}");
            app.MapHub<ChatHub>("/chathub");
            app.MapRazorPages();
            app.Run();
        }
    }
}
