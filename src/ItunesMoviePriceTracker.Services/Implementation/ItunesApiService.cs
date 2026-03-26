using ItunesMoviePriceTracker.Services.Interfaces;
using ItunesMoviePriceTracker.Shared.DTOs;
using System.Text.Json;

namespace ItunesMoviePriceTracker.Services.Implementation;

public class ItunesApiService(IHttpClientFactory httpClientFactory, string storeCountry) : IItunesApiService
{
    public async Task<ItunesMovieResult?> FetchMovieAsync(int trackId)
    {
        var url = $"https://itunes.apple.com/lookup?country={storeCountry}&id={trackId}";
        var client = httpClientFactory.CreateClient();

        var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ItunesLookupResponse>(json,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        var movie = result?.Results.FirstOrDefault();

        if (movie is not null && !string.IsNullOrEmpty(movie.ArtworkUrl60))
            movie.ArtworkUrl400 = movie.ArtworkUrl60[..^11] + "400x400bb.jpg";

        return movie;
    }

    private class ItunesLookupResponse
    {
        public int ResultCount { get; set; }
        public List<ItunesMovieResult> Results { get; set; } = new();
    }
}