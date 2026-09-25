using ItunesMoviePriceTracker.Repository.Entities;
using ItunesMoviePriceTracker.Repository.Repositories;
using ItunesMoviePriceTracker.Services.Interfaces;
using Microsoft.Extensions.Logging;

public class PriceUpdateService(IMovieRepository movieRepository,
    IMoviePriceRepository moviePriceRepository,
    IItunesApiService itunesApiService,
    INotificationService notificationService,
    IStoreSettingsProvider storeSettingsProvider,
    ILogger<PriceUpdateService> logger) : IPriceUpdateService
{
    private static readonly TimeSpan ApiCallDelay = TimeSpan.FromSeconds(3);

    public async Task<string> CheckForPriceUpdateAsync()
    {
        var store = await storeSettingsProvider.GetAsync();
        if (store is null)
        {
            logger.LogWarning("Price check skipped: no iTunes store has been selected");
            return $"Price check skipped, no store selected at {DateTime.Now:HH:mm:ss}";
        }

        var movies = (await movieRepository.GetMoviesForPriceCheckAsync()).ToList();

        if (movies.Count == 0)
        {
            logger.LogInformation("No movies due for price check");
            return $"No movies due for price check at {DateTime.Now:HH:mm:ss}";
        }

        logger.LogInformation("Starting price check for {Count} movies in store {CountryCode}",
            movies.Count, store.CountryCode);

        var updated = 0;
        foreach (var (index, movie) in movies.Index())
        {
            if (await TryCheckMoviePriceAsync(movie, store.CountryCode))
                updated++;

            if (index < movies.Count - 1)
                await Task.Delay(ApiCallDelay);
        }

        logger.LogInformation("Price check completed. Checked {Count} movies, updated {Updated} prices",
            movies.Count, updated);

        return $"Checked {movies.Count} movies, updated {updated} prices at {DateTime.Now:HH:mm:ss}";
    }

    /// <summary>
    /// Checks a single movie and never throws, so one failing movie cannot stop the whole run.
    /// </summary>
    /// <returns><c>true</c> if a new price was recorded.</returns>
    private async Task<bool> TryCheckMoviePriceAsync(Movie movie, string countryCode)
    {
        try
        {
            return await CheckMoviePriceAsync(movie, countryCode);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Price check failed for movie {TrackId}", movie.TrackId);
            return false;
        }
    }

    private async Task<bool> CheckMoviePriceAsync(Movie movie, string countryCode)
    {
        var result = await itunesApiService.FetchMovieAsync(movie.TrackId);

        if (result is null || result.TrackHdPrice == movie.TrackHdPrice)
        {
            await movieRepository.UpdateAsync(movie);
            return false;
        }

        await RecordNewPriceAsync(movie, result.TrackHdPrice, countryCode);
        await TrySendPriceAlertAsync(movie, result.TrackHdPrice);
        return true;
    }

    private async Task RecordNewPriceAsync(Movie movie, decimal newPrice, string countryCode)
    {
        await moviePriceRepository.AddAsync(new MoviePrice
        {
            Price = newPrice,
            CountryCode = countryCode,
            Date = DateTime.UtcNow,
            MovieTrackId = movie.TrackId
        });

        movie.TrackHdPrice = newPrice;
        await movieRepository.UpdateAsync(movie);
    }

    /// <summary>
    /// Sends a price alert if the new price is at or below the watch price.
    /// Failures are logged but never propagated, since the price is already saved.
    /// </summary>
    private async Task TrySendPriceAlertAsync(Movie movie, decimal newPrice)
    {
        if (movie.WatchPrice is not { } watchPrice || newPrice > watchPrice)
            return;

        try
        {
            await notificationService.SendPriceAlertAsync(
                movie.TrackName ?? movie.TrackId.ToString(),
                newPrice,
                watchPrice);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to send price alert for movie {TrackId}", movie.TrackId);
        }
    }
}