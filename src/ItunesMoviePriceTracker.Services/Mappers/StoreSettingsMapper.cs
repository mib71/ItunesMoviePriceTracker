using ItunesMoviePriceTracker.Repository.Entities;
using ItunesMoviePriceTracker.Shared.DTOs;

namespace ItunesMoviePriceTracker.Services.Mappers;

public static class StoreSettingsMapper
{
    public static StoreSettingsDto ToDto(StoreSettings settings) =>
        new(settings.CountryCode, settings.CurrencyCode, settings.Culture);
}