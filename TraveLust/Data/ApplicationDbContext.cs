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

        public DbSet<Message> Messages { get; set; }

        public DbSet<Itinerary> Itineraries { get; set; }

        public DbSet<PostInItinerary> PostInItineraries { get; set; }

        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserInGroupchat>()
            .HasKey(ug => new { ug.UserId, ug.GroupchatId });

            //generated with gitHub copilot
            modelBuilder.Entity<PostInItinerary>()
                .HasKey(pi => new { pi.PostId, pi.ItineraryId });

            modelBuilder.Entity<UserInGroupchat>()
            .HasOne(ab => ab.User)
            .WithMany(ab => ab.UserInGroupchats)
            .HasForeignKey(ab => ab.UserId);
            modelBuilder.Entity<UserInGroupchat>()
            .HasOne(ab => ab.Groupchat)
            .WithMany(ab => ab.UserInGroupchats)
            .HasForeignKey(ab => ab.GroupchatId);

            modelBuilder.Entity<PostInItinerary>()
          .HasOne(ab => ab.Post)
          .WithMany(ab => ab.PostInItineraries)
          .HasForeignKey(ab => ab.PostId);
            modelBuilder.Entity<PostInItinerary>()
            .HasOne(ab => ab.Itinerary)
            .WithMany(ab => ab.PostInItineraries)
            .HasForeignKey(ab => ab.ItineraryId);

            modelBuilder.Entity<Groupchat>()
                .HasOne(g => g.Itinerary)
                .WithOne(g => g.Groupchat)
                .HasForeignKey<Itinerary>(g => g.GroupchatId);

            modelBuilder.Entity<Vote>()
                .HasKey(v => new { v.UserId, v.PostId, v.ItineraryId });
        }
    }
}