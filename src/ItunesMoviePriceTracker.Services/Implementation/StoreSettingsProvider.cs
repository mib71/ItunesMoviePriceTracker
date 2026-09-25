using ItunesMoviePriceTracker.Repository.Repositories;
using ItunesMoviePriceTracker.Services.Interfaces;
using ItunesMoviePriceTracker.Services.Mappers;
using ItunesMoviePriceTracker.Shared.DTOs;

namespace ItunesMoviePriceTracker.Services.Implementation;

/// <summary>
/// Reads the selected store once and caches it for the lifetime of the process.
/// A missing selection is not cached, so completing setup takes effect without a restart.
/// </summary>
public class StoreSettingsProvider(IStoreSettingsRepository storeSettingsRepository) : IStoreSettingsProvider
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
}