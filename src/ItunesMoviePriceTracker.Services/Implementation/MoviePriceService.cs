using ItunesMoviePriceTracker.Repository.Repositories;
using ItunesMoviePriceTracker.Services.Interfaces;
using ItunesMoviePriceTracker.Services.Mappers;
using ItunesMoviePriceTracker.Shared.DTOs;

namespace ItunesMoviePriceTracker.Services.Implementation;

public class MoviePriceService(IMoviePriceRepository moviePriceRepository) : IMoviePriceService
{
    public async Task<IEnumerable<MoviePriceDto>> GetPriceHistoryAsync(int trackId)
    {
        var prices = await moviePriceRepository.GetByMovieIdAsync(trackId);
        return prices.Select(MoviePriceMapper.ToDto);
    }

    public async Task<MoviePriceDto?> GetLatestPriceAsync(int trackId)
    {
        var price = await moviePriceRepository.GetLatestByMovieIdAsync(trackId);
        return price is null ? null : MoviePriceMapper.ToDto(price);
    }

    public async Task<IEnumerable<MoviePriceDto>> GetTodaysChangesAsync()
    {
        var prices = await moviePriceRepository.GetTodaysChangesAsync();
        return prices.Select(MoviePriceMapper.ToDto);
    }
}