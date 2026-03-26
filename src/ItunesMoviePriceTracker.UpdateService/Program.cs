using ItunesMoviePriceTracker.Services.Extensions;
using ItunesMoviePriceTracker.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.SetBasePath(AppContext.BaseDirectory);
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
    })
    .ConfigureServices((context, services) =>
    {
        var connectionString = context.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        var throttleHours = context.Configuration.GetValue<int?>("PriceCheck:ThrottleHours") ?? 24;
        var storeCountry = context.Configuration.GetValue<string>("PriceCheck:StoreCountry") ?? "se";

        services.AddMovieServices(connectionString, throttleHours, storeCountry);
    })
    .Build();

await host.Services.ApplyMigrationsAsync();

var priceUpdateService = host.Services.GetRequiredService<IPriceUpdateService>();
var result = await priceUpdateService.CheckForPriceUpdateAsync();

Console.WriteLine(result);