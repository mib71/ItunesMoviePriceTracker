using ItunesMoviePriceTracker.Repository.Context;
using ItunesMoviePriceTracker.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace ItunesMoviePriceTracker.Repository.Repositories;

public class MovieRepository(IDbContextFactory<AppDbContext> contextFactory, int throttleHours = 24) : IMovieRepository
{
    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        return await context.Movies
            .AsNoTracking()
            .Include(m => m.Prices)
            .OrderBy(m => m.TrackHdPrice)
            .ToListAsync();
    }
         

    public async Task<Movie?> GetByIdAsync(long trackId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        return await context.Movies
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.TrackId == trackId);
    }
        

    public async Task<Movie?> GetByIdWithPricesAsync(long trackId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        return await context.Movies
            .AsNoTracking()
            .Include(m => m.Prices)
            .FirstOrDefaultAsync(m => m.TrackId == trackId);
    }

    public async Task AddAsync(Movie movie)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        await context.Movies.AddAsync(movie);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Movie movie)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        movie.LastChecked = DateTime.UtcNow;
        context.Movies.Update(movie);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long trackId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        var movie = await context.Movies.FindAsync(trackId);
        if (movie != null)
        {
            context.Movies.Remove(movie);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(long trackId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        return await context.Movies.AnyAsync(m => m.TrackId == trackId);
    }

    public async Task<IEnumerable<Movie>> GetMoviesForPriceCheckAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var threshold = DateTime.UtcNow.AddHours(-throttleHours);

        return await context.Movies
            .AsNoTracking()
            .Where(m => m.LastChecked <= threshold)
            .ToListAsync();
    }
}