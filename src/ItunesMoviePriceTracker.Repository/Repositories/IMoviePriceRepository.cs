using ItunesMoviePriceTracker.Repository.Entities;

namespace ItunesMoviePriceTracker.Repository.Repositories;

public interface IMoviePriceRepository
{
    Task<IEnumerable<MoviePrice>> GetByMovieIdAsync(long trackId);
    Task<MoviePrice?> GetLatestByMovieIdAsync(long trackId);
    Task<IEnumerable<MoviePrice>> GetTodaysChangesAsync();
    Task AddAsync(MoviePrice moviePrice);
}
