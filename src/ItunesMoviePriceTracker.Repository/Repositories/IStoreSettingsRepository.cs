using ItunesMoviePriceTracker.Repository.Entities;

namespace ItunesMoviePriceTracker.Repository.Repositories;

public interface IStoreSettingsRepository
{
    /// <summary>Returns the store settings row, or <c>null</c> if no store has been selected yet.</summary>
    Task<StoreSettings?> GetAsync();
}