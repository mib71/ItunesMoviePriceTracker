using ItunesMoviePriceTracker.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace ItunesMoviePriceTracker.Repository.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<MoviePrice> Prices => Set<MoviePrice>();
    public DbSet<StoreSettings> StoreSettings => Set<StoreSettings>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MoviePrice>()
            .HasOne(p => p.Movie)
            .WithMany(m => m.Prices)
            .HasForeignKey(p => p.MovieTrackId);

        modelBuilder.Entity<StoreSettings>(entity =>
        {
            entity.Property(s => s.Id).ValueGeneratedNever();
            entity.ToTable(table => table.HasCheckConstraint(
                "CK_StoreSettings_SingleRow",
                $"[Id] = {Entities.StoreSettings.SingletonId}"));
        });
    }
}