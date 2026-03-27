namespace ItunesMoviePriceTracker.Services.Interfaces;

public interface INotificationService
{
    Task SendPriceAlertAsync(string movieTitle, decimal currentPrice, decimal watchPrice);
}
