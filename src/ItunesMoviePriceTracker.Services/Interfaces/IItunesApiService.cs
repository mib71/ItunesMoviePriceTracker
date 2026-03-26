using ItunesMoviePriceTracker.Shared.DTOs;

namespace ItunesMoviePriceTracker.Services.Interfaces;

public interface IItunesApiService
{
    Task<ItunesMovieResult?> FetchMovieAsync(int trackId);
}