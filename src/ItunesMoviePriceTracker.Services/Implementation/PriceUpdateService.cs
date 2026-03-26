using ItunesMoviePriceTracker.Repository.Entities;
using ItunesMoviePriceTracker.Repository.Repositories;
using ItunesMoviePriceTracker.Services.Interfaces;

namespace ItunesMoviePriceTracker.Services.Implementation;

public class PriceUpdateService(
    IMovieRepository movieRepository,
    IMoviePriceRepository moviePriceRepository,
    IItunesApiService itunesApiService) : IPriceUpdateService
{
    public async Task<string> CheckForPriceUpdateAsync()
    {
        int updated = 0;
        var movies = (await movieRepository.GetMoviesForPriceCheckAsync()).ToList();

        if (movies.Count == 0)
            return $"No movies due for price check at {DateTime.Now:HH:mm:ss}";

        try
        {
            foreach (var movie in movies)
            {
                var result = await itunesApiService.FetchMovieAsync(movie.TrackId);

                if (result is not null && result.TrackHdPrice != movie.TrackHdPrice)
                {
                    await moviePriceRepository.AddAsync(new MoviePrice
                    {
                        Price = result.TrackHdPrice,
                        Date = DateTime.UtcNow,
                        MovieTrackId = movie.TrackId
                    });

                    movie.TrackHdPrice = result.TrackHdPrice;
                    updated++;
                }

                await movieRepository.UpdateAsync(movie);
                await Task.Delay(3000);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"iTunes price check failed.\n{ex}");
        }

        return $"Checked {movies.Count} movies, updated {updated} prices at {DateTime.Now:HH:mm:ss}";
    }
}