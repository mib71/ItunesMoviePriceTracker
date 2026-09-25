namespace ItunesMoviePriceTracker.Shared.DTOs;

/// <summary>An iTunes storefront (country store) the application can run against.</summary>
/// <param name="CountryCode">Two-letter country code used by the iTunes API, lower case.</param>
/// <param name="Name">Display name of the country.</param>
/// <param name="CurrencyCode">ISO 4217 code of the currency the store prices in.</param>
/// <param name="Culture">Culture name used to format prices from this store.</param>
public sealed record Storefront(string CountryCode, string Name, string CurrencyCode, string Culture);