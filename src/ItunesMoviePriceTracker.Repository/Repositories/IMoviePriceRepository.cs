using ItunesMoviePriceTracker.Repository.Entities;

namespace ItunesMoviePriceTracker.Repository.Repositories;

public interface IMoviePriceRepository
{
    Task<IEnumerable<MoviePrice>> GetByMovieIdAsync(int trackId);
    Task<MoviePrice?> GetLatestByMovieIdAsync(int trackId);
    Task<IEnumerable<MoviePrice>> GetTodaysChangesAsync();
    Task AddAsync(MoviePrice moviePrice);
}
