using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItunesMoviePriceTracker.Repository.Entities;

public class MoviePrice
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    /// <summary>Country code of the iTunes store the price was fetched from.</summary>
    [MaxLength(2)]
    public required string CountryCode { get; set; }

    public long MovieTrackId { get; set; }
    public Movie Movie { get; set; } = null!;
}