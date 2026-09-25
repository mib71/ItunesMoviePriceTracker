using ItunesMoviePriceTracker.Repository.Context;
using ItunesMoviePriceTracker.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace ItunesMoviePriceTracker.Repository.Repositories;

public class StoreSettingsRepository(IDbContextFactory<AppDbContext> contextFactory) : IStoreSettingsRepository
{
    /// <inheritdoc />
    public async Task<StoreSettings?> GetAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        return await context.StoreSettings
            .AsNoTracking()
            .SingleOrDefaultAsync(s => s.Id == StoreSettings.SingletonId);
    }
}