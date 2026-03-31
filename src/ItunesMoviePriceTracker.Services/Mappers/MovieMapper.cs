using ItunesMoviePriceTracker.Repository.Entities;
using ItunesMoviePriceTracker.Shared.DTOs;

namespace ItunesMoviePriceTracker.Services.Mappers;

public static class MovieMapper
{
    public static MovieDto ToDto(Movie movie) => new()
    {
        TrackId = movie.TrackId,
        TrackName = movie.TrackName,
        ReleaseDate = movie.ReleaseDate,
        ArtistName = movie.ArtistName,
        LongDescription = movie.LongDescription,
        ArtworkUrl60 = movie.ArtworkUrl60,
        ArtworkUrl400 = movie.ArtworkUrl400,
        TrackHdPrice = movie.TrackHdPrice,
        WatchPrice = movie.WatchPrice,
        LastChecked = movie.LastChecked,
        Prices = movie.Prices.Select(MoviePriceMapper.ToDto).ToList()
    };

    public static Movie ToEntity(ItunesMovieResult result, decimal? watchPrice = null) => new()
    {
        TrackId = result.TrackId,
        TrackName = result.TrackName,
        ReleaseDate = result.ReleaseDate,
        ArtistName = result.ArtistName,
        LongDescription = result.LongDescription,
        ArtworkUrl60 = result.ArtworkUrl60,
        ArtworkUrl400 = result.ArtworkUrl400,
        TrackHdPrice = result.TrackHdPrice,
        LastChecked = DateTime.UtcNow,
        WatchPrice = watchPrice,
        Prices = new List<MoviePrice>
        {
            new() { Price = result.TrackHdPrice, Date = DateTime.UtcNow }
        }
    };
}