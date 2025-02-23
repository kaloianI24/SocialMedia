using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Areas.Identity.Data;
using SocialMedia.Data.Models;
using System.Reflection.Emit;

namespace SocialMedia.Data
{
    public class SocialMediaDbContext : IdentityDbContext<SocialMediaUser>
    {

        public DbSet<CloudResource> Attachments { get; set; }
        public DbSet<SocialMediaComment> Comments { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<SocialMediaPost> Posts { get; set; }
        public DbSet<SocialMediaReaction> Reactions { get; set; }
        public DbSet<SocialMediaRole> SocialMediaRoles { get; set; }
        public DbSet<SocialMediaTag> Tags { get; set; }
        public bool AcceptedTerms { get; set; }


        public SocialMediaDbContext(DbContextOptions<SocialMediaDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {        
            builder.Entity<SocialMediaUser>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.CreatedBy)
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SocialMediaPost>()
               .HasMany(p => p.TaggedUsers)
               .WithMany(u => u.TaggedPosts)
               .UsingEntity(j => j.ToTable("PostTaggedUsers"));
                        
            builder.Entity<SocialMediaPost>()
                .HasOne(p => p.UpdatedBy)
                .WithMany()
                .HasForeignKey(p => p.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SocialMediaPost>()
                .HasOne(p => p.DeletedBy)
                .WithMany()
                .HasForeignKey(p => p.DeletedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SocialMediaComment>()
                .HasMany(c => c.Attachments)
                .WithMany();

            builder.Entity<SocialMediaUser>()
                .HasOne(u => u.ProfilePicture);

            builder.Entity<SocialMediaPost>()
                .HasMany(p => p.Attachments)
            .WithMany();

            builder.Entity<SocialMediaUser>()
            .HasMany(u => u.Friends)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "UserFriends",
                j => j.HasOne<SocialMediaUser>().WithMany().HasForeignKey("FriendId"),
                j => j.HasOne<SocialMediaUser>().WithMany().HasForeignKey("UserId"),
                j => j.ToTable("UserFriends")
            );

            builder.Entity<SocialMediaUser>()
            .HasMany(u => u.Following)
            .WithMany(u => u.Followers)
            .UsingEntity<Dictionary<string, object>>(
                "UserFollows",
                j => j.HasOne<SocialMediaUser>()
                      .WithMany()
                      .HasForeignKey("FollowedUserId")
                      .OnDelete(DeleteBehavior.Restrict),
                j => j.HasOne<SocialMediaUser>()
                      .WithMany()
                      .HasForeignKey("FollowerId")
                      .OnDelete(DeleteBehavior.Cascade)
            );

            builder.Entity<FriendRequest>()
            .HasOne(fr => fr.CreatedBy)
            .WithMany(u => u.SentFriendRequests)
            .HasForeignKey(fr => fr.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FriendRequest>()
            .HasOne(fr => fr.Receiver)
            .WithMany(u => u.ReceivedFriendRequests)
            .HasForeignKey(fr => fr.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FriendRequest>()
                .HasOne(fr => fr.CreatedBy)
                .WithMany(u => u.SentFriendRequests)
                .HasForeignKey(fr => fr.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FriendRequest>()
                .HasOne(fr => fr.Receiver)
                .WithMany(u => u.ReceivedFriendRequests)
                .HasForeignKey(fr => fr.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FriendRequest>()
                .HasOne(fr => fr.UpdatedBy)
                .WithMany()
                .HasForeignKey(fr => fr.UpdatedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FriendRequest>()
                .HasOne(fr => fr.DeletedBy)
                .WithMany()
                .HasForeignKey(fr => fr.DeletedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //optionsBuilder.UseMySql(
        //    //    "Server=127.0.0.1;Database=SocialMedia;User=root;Password=Kaloianivanov24!;",
        //    //    new MySqlServerVersion(new Version(8, 0, 21)),
        //    //    b => b.MigrationsAssembly("SocialMedia.Data"));
        //}
    }
}
