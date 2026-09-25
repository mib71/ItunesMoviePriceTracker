using ItunesMoviePriceTracker.Shared.DTOs;

namespace ItunesMoviePriceTracker.Services.Interfaces;

/// <summary>Provides the iTunes storefront selected for this installation.</summary>
public interface IStoreSettingsProvider
{
    /// <summary>Returns the active store, or <c>null</c> if setup has not been completed.</summary>
    Task<StoreSettingsDto?> GetAsync();

    /// <summary>Returns the active store.</summary>
    /// <exception cref="InvalidOperationException">No store has been selected yet.</exception>
    Task<StoreSettingsDto> GetRequiredAsync();

    /// <summary>Selects the iTunes store for this installation. Can only be done once.</summary>
    /// <param name="countryCode">Country code of a store in the storefront catalog.</param>
    /// <returns><c>true</c> if the store was selected; <c>false</c> if a store was already selected.</returns>
    /// <exception cref="ArgumentException">The country is not in the storefront catalog.</exception>
    Task<bool> SelectAsync(string countryCode);
}