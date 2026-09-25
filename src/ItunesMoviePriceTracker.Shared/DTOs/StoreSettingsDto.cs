namespace ItunesMoviePriceTracker.Shared.DTOs;

/// <summary>The iTunes storefront selected for this installation.</summary>
/// <param name="CountryCode">Two-letter country code used by the iTunes API, lower case.</param>
/// <param name="CurrencyCode">ISO 4217 code of the store's currency.</param>
/// <param name="Culture">Culture name used to format prices from this store.</param>
public sealed record StoreSettingsDto(string CountryCode, string CurrencyCode, string Culture);