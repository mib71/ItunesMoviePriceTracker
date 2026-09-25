using ItunesMoviePriceTracker.Repository.Context;
using ItunesMoviePriceTracker.Repository.Entities;
using Microsoft.Data.SqlClient;
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

    /// <inheritdoc />
    public async Task<bool> TryAddAsync(StoreSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        await using var context = await contextFactory.CreateDbContextAsync();

        if (await context.StoreSettings.AnyAsync())
            return false;

        context.StoreSettings.Add(settings);

        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2627 })
        {
            // Another request created the row between the check and the insert.
            return false;
        }
    }
}