using ItunesMoviePriceTracker.Repository.Entities;
using ItunesMoviePriceTracker.Repository.Repositories;
using ItunesMoviePriceTracker.Services.Interfaces;
using ItunesMoviePriceTracker.Services.Mappers;
using ItunesMoviePriceTracker.Services.Storefronts;
using ItunesMoviePriceTracker.Shared.DTOs;
using Microsoft.Extensions.Logging;

namespace ItunesMoviePriceTracker.Services.Implementation;

/// <summary>
/// Reads the selected store once and caches it for the lifetime of the process.
/// A missing selection is not cached, so completing setup takes effect without a restart.
/// </summary>
public class StoreSettingsProvider(IStoreSettingsRepository storeSettingsRepository,
    ILogger<StoreSettingsProvider> logger) : IStoreSettingsProvider
{
    private StoreSettingsDto? _cached;

    /// <inheritdoc />
    public async Task<StoreSettingsDto?> GetAsync()
    {
        if (_cached is not null)
            return _cached;

        var settings = await storeSettingsRepository.GetAsync();
        if (settings is null)
            return null;

        _cached = StoreSettingsMapper.ToDto(settings);
        return _cached;
    }

    /// <inheritdoc />
    public async Task<StoreSettingsDto> GetRequiredAsync() =>
        await GetAsync()
            ?? throw new InvalidOperationException(
                "No iTunes store has been selected. Complete setup in the web app first.");

    /// <inheritdoc />
    public async Task<bool> SelectAsync(string countryCode)
    {
        var storefront = StorefrontCatalog.Find(countryCode)
            ?? throw new ArgumentException($"'{countryCode}' is not a supported iTunes store.", nameof(countryCode));

        var settings = new StoreSettings
        {
            CountryCode = storefront.CountryCode,
            CurrencyCode = storefront.CurrencyCode,
            Culture = storefront.Culture,
            SelectedAt = DateTime.UtcNow
        };

        if (!await storeSettingsRepository.TryAddAsync(settings))
            return false;

        _cached = StoreSettingsMapper.ToDto(settings);
        logger.LogInformation("iTunes store selected: {CountryCode} ({CurrencyCode})",
            storefront.CountryCode, storefront.CurrencyCode);

        return true;
    }
}