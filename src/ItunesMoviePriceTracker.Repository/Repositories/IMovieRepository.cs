using ItunesMoviePriceTracker.Repository.Entities;

namespace ItunesMoviePriceTracker.Repository.Repositories;

public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetAllAsync();
    Task<Movie?> GetByIdAsync(long trackId);
    Task<Movie?> GetByIdWithPricesAsync(long trackId);
    Task AddAsync(Movie movie);
    Task UpdateAsync(Movie movie);
    Task DeleteAsync(long trackId);
    Task<bool> ExistsAsync(long trackId);
    Task<IEnumerable<Movie>> GetMoviesForPriceCheckAsync();
}