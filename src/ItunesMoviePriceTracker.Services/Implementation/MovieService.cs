using ItunesMoviePriceTracker.Repository.Repositories;
using ItunesMoviePriceTracker.Services.Interfaces;
using ItunesMoviePriceTracker.Services.Mappers;
using ItunesMoviePriceTracker.Shared.DTOs;

namespace ItunesMoviePriceTracker.Services.Implementation;

public class MovieService(IMovieRepository movieRepository,
    IItunesApiService itunesApiService,
    INotificationService notificationService) : IMovieService
{
    public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
    {
        var movies = await movieRepository.GetAllAsync();
        return movies.Select(MovieMapper.ToDto);
    }

    public async Task<MovieDto?> GetMovieAsync(int trackId)
    {
        var movie = await movieRepository.GetByIdAsync(trackId);
        return movie is null ? null : MovieMapper.ToDto(movie);
    }

    public async Task<MovieDto?> GetMovieWithPricesAsync(int trackId)
    {
        var movie = await movieRepository.GetByIdWithPricesAsync(trackId);
        return movie is null ? null : MovieMapper.ToDto(movie);
    }

    public async Task<bool> AddMovieAsync(int trackId, decimal? watchPrice)
    {
        if (await movieRepository.ExistsAsync(trackId)) return false;

        var result = await itunesApiService.FetchMovieAsync(trackId);
        if (result is null) return false;

        await movieRepository.AddAsync(MovieMapper.ToEntity(result, watchPrice));

        if (watchPrice.HasValue && result.TrackHdPrice <= watchPrice.Value)
        {
            await notificationService.SendPriceAlertAsync(result.TrackName ?? "Unknown Movie", result.TrackHdPrice, watchPrice.Value);
        }

        return true;
    }

    public async Task UpdateWatchPriceAsync(int trackId, decimal? watchPrice)
    {
        var movie = await movieRepository.GetByIdAsync(trackId);
        if (movie is null) return;

        movie.WatchPrice = watchPrice;
        await movieRepository.UpdateAsync(movie);
    }

    public async Task<bool> DeleteMovieAsync(int trackId)
    {
        if (!await movieRepository.ExistsAsync(trackId)) return false;
        await movieRepository.DeleteAsync(trackId);
        return true;
    }
}