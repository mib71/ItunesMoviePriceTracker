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
}