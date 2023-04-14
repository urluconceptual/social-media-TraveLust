using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TraveLust.Models;

namespace TraveLust.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Groupchat> Groupchats { get; set; }

        public DbSet<UserInGroupchat> UserInGroupchats { get; set; }

        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserInGroupchat>()
            .HasKey(ug => new { ug.UserId, ug.GroupchatId });

            modelBuilder.Entity<UserInGroupchat>()
            .HasOne(ab => ab.User)
            .WithMany(ab => ab.UserInGroupchats)
            .HasForeignKey(ab => ab.UserId);
            modelBuilder.Entity<UserInGroupchat>()
            .HasOne(ab => ab.Groupchat)
            .WithMany(ab => ab.UserInGroupchats)
            .HasForeignKey(ab => ab.GroupchatId);
        }
    }
}