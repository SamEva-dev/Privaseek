#if ANDROID
using Android.App.Usage;
#endif
using Privaseek.Applications.Contracts;
using Privaseek.Domain.Entities;
using SQLite;

namespace Privaseek.Infrastructure.Providers;

public class AppUsageProvider : IAppUsageIndexer
{
    private readonly SQLiteAsyncConnection _db;

    public AppUsageProvider(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
        _db.CreateTableAsync<AppUsageEntry>().Wait();
    }

    public async Task IndexAsync(CancellationToken ct = default)
    {
#if ANDROID
        var usm = (UsageStatsManager)
            Android.App.Application.Context.GetSystemService("usagestats");
        var now = Java.Lang.JavaSystem.CurrentTimeMillis();
        var events = usm.QueryUsageStats(
            UsageStatsInterval.Daily, 0, now);

        var list = events
            .Select(e => new AppUsageEntry
            {
                PackageName = e.PackageName,
                AppName = e.PackageName, // tu peux utiliser PackageManager pour label
                LastTimeUsed = DateTimeOffset.FromUnixTimeMilliseconds(e.LastTimeUsed).UtcDateTime
            })
            .ToList();
#else
        var list = new List<AppUsageEntry>();
#endif

        await _db.DeleteAllAsync<AppUsageEntry>();
        if (list.Count > 0)
            await _db.InsertAllAsync(list);
    }
}
