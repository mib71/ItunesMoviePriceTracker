using System.ComponentModel.DataAnnotations;

namespace ItunesMoviePriceTracker.Repository.Entities;

/// <summary>
/// Single-row table holding the iTunes storefront selected for this installation.
/// The row is absent until setup has been completed.
/// </summary>
public class StoreSettings
{
    /// <summary>The only allowed primary key value, enforced by a check constraint.</summary>
    public const int SingletonId = 1;

    public int Id { get; set; } = SingletonId;

    [MaxLength(2)]
    public required string CountryCode { get; set; }

    [MaxLength(3)]
    public required string CurrencyCode { get; set; }

    [MaxLength(10)]
    public required string Culture { get; set; }

    public DateTime SelectedAt { get; set; }
}