namespace ItunesMoviePriceTracker.Shared.DTOs;

public class ItunesMovieResult
{
    public int TrackId { get; set; }
    public string? TrackName { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public string? ArtistName { get; set; }
    public string? LongDescription { get; set; }
    public string? ArtworkUrl60 { get; set; }
    public string? ArtworkUrl400 { get; set; }
    public decimal TrackHdPrice { get; set; }
}