using ItunesMoviePriceTracker.Shared.DTOs;

namespace ItunesMoviePriceTracker.Services.Interfaces;

public interface IMovieService
{
    Task<IEnumerable<MovieDto>> GetAllMoviesAsync();
    Task<MovieDto?> GetMovieAsync(int trackId);
    Task<MovieDto?> GetMovieWithPricesAsync(int trackId);
    Task<bool> AddMovieAsync(int trackId, decimal? watchPrice);
    Task UpdateWatchPriceAsync(int trackId, decimal? watchPrice);
    Task<bool> DeleteMovieAsync(int trackId);
}