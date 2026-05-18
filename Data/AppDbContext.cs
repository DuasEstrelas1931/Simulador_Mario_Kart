using Microsoft.EntityFrameworkCore;
using Simulador_Mario_Kart.Models;

namespace Simulador_Mario_Kart.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Race> Races => Set<Race>();
        public DbSet<RaceResult> RaceResults => Set<RaceResult>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.Username).IsUnique();
            });

            modelBuilder.Entity<RaceResult>(entity =>
            {
                entity.HasOne(r => r.Race)
                    .WithMany(r => r.Results)
                    .HasForeignKey(r => r.RaceId);

                entity.HasOne(r => r.User)
                    .WithMany(u => u.RaceResults)
                    .HasForeignKey(r => r.UserId);
            });
        }
    }
}
