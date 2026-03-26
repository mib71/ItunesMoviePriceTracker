using ItunesMoviePriceTracker.Repository.Context;
using ItunesMoviePriceTracker.Repository.Repositories;
using ItunesMoviePriceTracker.Services.Implementation;
using ItunesMoviePriceTracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ItunesMoviePriceTracker.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMovieServices(this IServiceCollection services, string connectionString, int throttleHours = 24)
    {
        // Database
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Repositories
        services.AddScoped<IMovieRepository>(sp =>
        new MovieRepository(
            sp.GetRequiredService<AppDbContext>(),
            throttleHours));
        services.AddScoped<IMoviePriceRepository, MoviePriceRepository>();

        // Services
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IMoviePriceService, MoviePriceService>();
        services.AddScoped<IPriceUpdateService, PriceUpdateService>();
        services.AddScoped<IItunesApiService, ItunesApiService>();

        // HttpClient
        services.AddHttpClient();

        return services;
    }

    public static async Task ApplyMigrationsAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();
    }
}