using ItunesMoviePriceTracker.Repository.Context;
using ItunesMoviePriceTracker.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace ItunesMoviePriceTracker.Repository.Repositories;

public class MoviePriceRepository(AppDbContext context) : IMoviePriceRepository
{
    public async Task<IEnumerable<MoviePrice>> GetByMovieIdAsync(int trackId)
        => await context.Prices
            .AsNoTracking()
            .Where(p => p.MovieTrackId == trackId)
            .OrderByDescending(p => p.Date)
            .ToListAsync();

    public async Task<MoviePrice?> GetLatestByMovieIdAsync(int trackId)
        => await context.Prices
            .AsNoTracking()
            .Where(p => p.MovieTrackId == trackId)
            .OrderByDescending(p => p.Date)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<MoviePrice>> GetTodaysChangesAsync()
    {
        var today = DateTime.UtcNow.Date;
        return await context.Prices
            .AsNoTracking()
            .Include(p => p.Movie)
            .Where(p => p.Date >= today)
            .ToListAsync();
    }

    public async Task AddAsync(MoviePrice moviePrice)
    {
        await context.Prices.AddAsync(moviePrice);
        await context.SaveChangesAsync();
    }
}