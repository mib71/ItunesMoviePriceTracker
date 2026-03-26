using ItunesMoviePriceTracker.Shared.DTOs;

namespace ItunesMoviePriceTracker.Services.Interfaces;

public interface IMoviePriceService
{
    Task<IEnumerable<MoviePriceDto>> GetPriceHistoryAsync(int trackId);
    Task<MoviePriceDto?> GetLatestPriceAsync(int trackId);
    Task<IEnumerable<MoviePriceDto>> GetTodaysChangesAsync();
}