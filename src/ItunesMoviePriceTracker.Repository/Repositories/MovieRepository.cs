using ItunesMoviePriceTracker.Repository.Context;
using ItunesMoviePriceTracker.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace ItunesMoviePriceTracker.Repository.Repositories;

public class MovieRepository(AppDbContext context, int throttleHours = 24) : IMovieRepository
{
    public async Task<IEnumerable<Movie>> GetAllAsync()
        => await context.Movies.Include(m => m.Prices).OrderBy(m => m.TrackHdPrice).ToListAsync();

    public async Task<Movie?> GetByIdAsync(int trackId)
        => await context.Movies.FindAsync(trackId);

    public async Task<Movie?> GetByIdWithPricesAsync(int trackId)
        => await context.Movies.Include(m => m.Prices).FirstOrDefaultAsync(m => m.TrackId == trackId);

    public async Task AddAsync(Movie movie)
    {
        await context.Movies.AddAsync(movie);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Movie movie)
    {
        movie.LastChecked = DateTime.UtcNow;
        context.Movies.Update(movie);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int trackId)
    {
        var movie = await context.Movies.FindAsync(trackId);
        if (movie != null)
        {
            context.Movies.Remove(movie);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int trackId)
        => await context.Movies.AnyAsync(m => m.TrackId == trackId);

    public async Task<IEnumerable<Movie>> GetMoviesForPriceCheckAsync()
    {
        var threshold = DateTime.UtcNow.AddHours(-throttleHours);
        return await context.Movies
            .Where(m => m.LastChecked <= threshold)
            .ToListAsync();
    }
}