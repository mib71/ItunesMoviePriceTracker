using ItunesMoviePriceTracker.Repository.Context;
using ItunesMoviePriceTracker.Services.Extensions;
using ItunesMoviePriceTracker.Web.Components;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

var throttleHours = builder.Configuration.GetValue<int>("PriceCheck:ThrottleHours");
builder.Services.AddMovieServices(connectionString, throttleHours);

var app = builder.Build();

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

// Apply migrations on startup
await app.Services.ApplyMigrationsAsync();

app.Run();