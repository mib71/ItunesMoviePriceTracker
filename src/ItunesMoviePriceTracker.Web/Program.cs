using ItunesMoviePriceTracker.Services.Extensions;
using ItunesMoviePriceTracker.Web.Components;
using ItunesMoviePriceTracker.Web.Configuration;
using Microsoft.AspNetCore.DataProtection;
using Radzen;
using Serilog;

// ---- Logging ----
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
    .WriteTo.File(
        path: "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
    .WriteTo.File(
        path: "logs/app-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// ---- Configuration ----
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var keyPath = builder.Configuration["DataProtection:KeyPath"]
    ?? throw new InvalidOperationException("DataProtection:KeyPath not found.");
var throttleHours = builder.Configuration.GetValue<int?>("PriceCheck:ThrottleHours") ?? 24;
var storeCountry = builder.Configuration.GetValue<string>("PriceCheck:StoreCountry") ?? "se";
builder.Services.Configure<UiSettings>(builder.Configuration.GetSection("UiPolling"));

// ---- Services ----
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keyPath))
    .SetApplicationName("ItunesMoviePriceTracker");
builder.Services.AddMovieServices(connectionString, throttleHours, storeCountry);
builder.Services.AddRadzenComponents();
builder.Services.AddServerSideBlazor(options =>
{
    options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(30);
});

// ---- Build ----
Log.Information("ItunesMoviePriceTracker v{Version} starting up",
    typeof(Program).Assembly.GetName().Version);

var app = builder.Build();

// ---- Middleware ----
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// ---- Startup tasks ----
await app.Services.ApplyMigrationsAsync();

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStopping.Register(() =>
{
    Log.Information("ItunesMoviePriceTracker shutting down");
    Log.CloseAndFlush();
});

app.Run();