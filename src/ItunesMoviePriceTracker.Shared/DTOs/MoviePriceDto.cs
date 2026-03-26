namespace ItunesMoviePriceTracker.Shared.DTOs;

public class MoviePriceDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
    public int MovieTrackId { get; set; }
}