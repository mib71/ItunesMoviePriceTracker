using ItunesMoviePriceTracker.Repository.Entities;

namespace ItunesMoviePriceTracker.Repository.Repositories;

public interface IStoreSettingsRepository
{
    /// <summary>Returns the store settings row, or <c>null</c> if no store has been selected yet.</summary>
    Task<StoreSettings?> GetAsync();

    /// <summary>
    /// Saves the store settings if no store has been selected yet.
    /// </summary>
    /// <returns><c>true</c> if the row was created; <c>false</c> if a store was already selected.</returns>
    Task<bool> TryAddAsync(StoreSettings settings);
}