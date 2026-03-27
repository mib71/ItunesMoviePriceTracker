using ItunesMoviePriceTracker.Repository.Entities;
using ItunesMoviePriceTracker.Repository.Repositories;
using ItunesMoviePriceTracker.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace ItunesMoviePriceTracker.Services.Implementation;

public class PriceUpdateService(IMovieRepository movieRepository,
    IMoviePriceRepository moviePriceRepository,
    IItunesApiService itunesApiService,
    INotificationService notificationService,
    ILogger<PriceUpdateService> logger) : IPriceUpdateService
{
    public async Task<string> CheckForPriceUpdateAsync()
    {
        int updated = 0;
        var movies = (await movieRepository.GetMoviesForPriceCheckAsync()).ToList();

        if (movies.Count == 0)
        {
            logger.LogInformation("No movies due for price check at {Time}", DateTime.Now);
            return $"No movies due for price check at {DateTime.Now:HH:mm:ss}";
        }

        logger.LogInformation("Starting price check for {Count} movies", movies.Count);

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

                    // Send notification if price drops below watch price
                    if (movie.WatchPrice.HasValue && result.TrackHdPrice <= movie.WatchPrice.Value)
                    {
                        await notificationService.SendPriceAlertAsync(
                            movie.TrackName ?? movie.TrackId.ToString(),
                            result.TrackHdPrice,
                            movie.WatchPrice.Value);
                    }

                    movie.TrackHdPrice = result.TrackHdPrice;
                    updated++;
                }

                await movieRepository.UpdateAsync(movie);
                await Task.Delay(3000);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "iTunes price check failed");
        }

        logger.LogInformation("Price check completed. Checked {Count} movies, updated {Updated} prices",
            movies.Count, updated);

        return $"Checked {movies.Count} movies, updated {updated} prices at {DateTime.Now:HH:mm:ss}";
    }
}