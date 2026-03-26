namespace ItunesMoviePriceTracker.Services.Interfaces;

public interface IPriceUpdateService
{
    Task<string> CheckForPriceUpdateAsync();
}