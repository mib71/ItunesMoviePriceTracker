using ItunesMoviePriceTracker.Repository.Context;
using ItunesMoviePriceTracker.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace ItunesMoviePriceTracker.Repository.Repositories;

public class MoviePriceRepository(IDbContextFactory<AppDbContext> contextFactory) : IMoviePriceRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<MoviePrice>> GetByMovieIdAsync(long trackId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        return await context.Prices
            .AsNoTracking()
            .Where(p => p.MovieTrackId == trackId)
            .OrderByDescending(p => p.Date)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<MoviePrice?> GetLatestByMovieIdAsync(long trackId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        return await context.Prices
            .AsNoTracking()
            .Where(p => p.MovieTrackId == trackId)
            .OrderByDescending(p => p.Date)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<MoviePrice>> GetTodaysChangesAsync()
    {
        var today = DateTime.UtcNow.Date;

        await using var context = await contextFactory.CreateDbContextAsync();

        return await context.Prices
            .AsNoTracking()
            .Include(p => p.Movie)
            .Where(p => p.Date >= today)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task AddAsync(MoviePrice moviePrice)
    {
        ArgumentNullException.ThrowIfNull(moviePrice);

        await using var context = await contextFactory.CreateDbContextAsync();

        context.Prices.Add(moviePrice);
        await context.SaveChangesAsync();
    }
}