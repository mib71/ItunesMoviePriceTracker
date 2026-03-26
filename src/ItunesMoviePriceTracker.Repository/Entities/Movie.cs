using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItunesMoviePriceTracker.Repository.Entities;

public class Movie
{
    [Key]
    public int TrackId { get; set; }
    [MaxLength(500)]
    public string? TrackName { get; set; }
    public DateTime? ReleaseDate { get; set; }
    [MaxLength(255)]
    public string? ArtistName { get; set; }
    [MaxLength(2000)]
    public string? LongDescription { get; set; }
    [MaxLength(500)]
    public string? ArtworkUrl60 { get; set; }
    [MaxLength(500)]
    public string? ArtworkUrl400 { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TrackHdPrice { get; set; }

    public DateTime LastChecked { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? WatchPrice { get; set; }

    public List<MoviePrice> Prices { get; set; } = new();
}
