using ItunesMoviePriceTracker.Repository.Entities;
using ItunesMoviePriceTracker.Shared.DTOs;

namespace ItunesMoviePriceTracker.Services.Mappers;

public static class MoviePriceMapper
{
    public static MoviePriceDto ToDto(MoviePrice price) => new()
    {
        Id = price.Id,
        Date = price.Date,
        Price = price.Price,
        MovieTrackId = price.MovieTrackId,
        TrackName = price.Movie?.TrackName
    };

    public static MoviePrice ToEntity(MoviePriceDto dto) => new()
    {
        Id = dto.Id,
        Date = dto.Date,
        Price = dto.Price,
        MovieTrackId = dto.MovieTrackId
    };
}