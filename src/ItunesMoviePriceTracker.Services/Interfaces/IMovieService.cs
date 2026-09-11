using ItunesMoviePriceTracker.Shared.DTOs;

namespace ItunesMoviePriceTracker.Services.Interfaces;

public interface IMovieService
{
    Task<IEnumerable<MovieDto>> GetAllMoviesAsync();
    Task<MovieDto?> GetMovieAsync(long trackId);
    Task<MovieDto?> GetMovieWithPricesAsync(long trackId);
    Task<bool> AddMovieAsync(long trackId, decimal? watchPrice);
    Task UpdateWatchPriceAsync(long trackId, decimal? watchPrice);
    Task<bool> DeleteMovieAsync(long trackId);
}