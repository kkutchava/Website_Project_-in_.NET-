using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wheat.Models;


namespace Wheat.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) //fluentu api
        {
            /*keys of Identity tables are mapped in
             * OnModelCreating method of IdentityDbContext
             * and if this method is not called, you
             * will end up getting the error */
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PostTag>()
                .HasKey(pt => new { pt.PostId, pt.TagId });

            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.PostsTags)
                .HasForeignKey(pt => pt.PostId);

            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.PostsTags)
                .HasForeignKey(pt => pt.TagId);
        }


        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTag> PostsTags { get; set; }

        public DbSet<Contact> contacts { get; set; }
        public DbSet<Invitations> invitations { get; set; }
        public DbSet<Wheat.Models.PostTagMod>? PostTagMod { get; set; }
    }
}