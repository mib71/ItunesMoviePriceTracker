using System.ComponentModel.DataAnnotations.Schema;

namespace ItunesMoviePriceTracker.Repository.Entities;

public class MoviePrice
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public int MovieTrackId { get; set; }
    public Movie Movie { get; set; } = null!;
}