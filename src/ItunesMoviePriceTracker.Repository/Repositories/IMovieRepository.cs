using ItunesMoviePriceTracker.Repository.Entities;

namespace ItunesMoviePriceTracker.Repository.Repositories;

public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetAllAsync();
    Task<Movie?> GetByIdAsync(int trackId);
    Task<Movie?> GetByIdWithPricesAsync(int trackId);
    Task AddAsync(Movie movie);
    Task UpdateAsync(Movie movie);
    Task DeleteAsync(int trackId);
    Task<bool> ExistsAsync(int trackId);
    Task<IEnumerable<Movie>> GetMoviesForPriceCheckAsync();
}