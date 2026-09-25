using ItunesMoviePriceTracker.Shared.DTOs;

namespace ItunesMoviePriceTracker.Services.Storefronts;

/// <summary>Built-in catalog of the iTunes storefronts a user can select during setup.</summary>
public static class StorefrontCatalog
{
    /// <summary>All supported storefronts, in display order.</summary>
    public static IReadOnlyList<Storefront> All { get; } =
    [
        new("at", "Austria", "EUR", "de-AT"),
        new("be", "Belgium", "EUR", "nl-BE"),
        new("ca", "Canada", "CAD", "en-CA"),
        new("dk", "Denmark", "DKK", "da-DK"),
        new("fi", "Finland", "EUR", "fi-FI"),
        new("fr", "France", "EUR", "fr-FR"),
        new("de", "Germany", "EUR", "de-DE"),
        new("ie", "Ireland", "EUR", "en-IE"),
        new("it", "Italy", "EUR", "it-IT"),
        new("nl", "Netherlands", "EUR", "nl-NL"),
        new("no", "Norway", "NOK", "nb-NO"),
        new("pl", "Poland", "PLN", "pl-PL"),
        new("pt", "Portugal", "EUR", "pt-PT"),
        new("es", "Spain", "EUR", "es-ES"),
        new("se", "Sweden", "SEK", "sv-SE"),
        new("ch", "Switzerland", "CHF", "de-CH"),
        new("gb", "United Kingdom", "GBP", "en-GB"),
        new("us", "United States", "USD", "en-US"),
    ];

    /// <summary>Finds a storefront by country code, ignoring case and surrounding whitespace.</summary>
    /// <returns>The storefront, or <c>null</c> if the country is not supported.</returns>
    public static Storefront? Find(string? countryCode) =>
        string.IsNullOrWhiteSpace(countryCode)
            ? null
            : All.FirstOrDefault(s => s.CountryCode.Equals(countryCode.Trim(), StringComparison.OrdinalIgnoreCase));
}