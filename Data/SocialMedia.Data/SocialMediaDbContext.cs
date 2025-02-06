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
        public DbSet<Comment> Comments { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<SocialMediaRole> SocialMediaRoles { get; set; }
        public DbSet<Tag> Tags { get; set; }


        public SocialMediaDbContext(DbContextOptions<SocialMediaDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SocialMediaUser>()
               .HasMany(u => u.Posts) // SocialMediaUser има много Post обекти
               .WithOne() // Post има един SocialMediaUser
               .HasForeignKey("SocialMediaUserId"); // Външен ключ в Post

            // Конфигуриране на много-към-много връзка между Post и SocialMediaUser (TaggedUsers)
            builder.Entity<Post>()
               .HasMany(p => p.TaggedUsers) // Post има много TaggedUsers
               .WithMany(u => u.TaggedPosts) // SocialMediaUser има много TaggedPosts
               .UsingEntity(j => j.ToTable("PostTaggedUsers"));

           builder.Entity<Post>()
        .HasOne(p => p.CreatedBy)
        .WithMany()
        .HasForeignKey(p => p.CreatedById)
        .OnDelete(DeleteBehavior.Restrict); // Забранява изтриване, ако има зависими записи

            // Конфигуриране на връзката между Post и SocialMediaUser (UpdatedBy)
            builder.Entity<Post>()
                .HasOne(p => p.UpdatedBy)
                .WithMany()
                .HasForeignKey(p => p.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            // Конфигуриране на връзката между Post и SocialMediaUser (DeletedBy)
            builder.Entity<Post>()
                .HasOne(p => p.DeletedBy)
                .WithMany()
                .HasForeignKey(p => p.DeletedById)
                .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(builder);
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
