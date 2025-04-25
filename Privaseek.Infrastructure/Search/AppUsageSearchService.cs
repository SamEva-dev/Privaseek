using Privaseek.Applications.Contracts;
using Privaseek.Domain.Entities;
using Privaseek.Domain.Enums;
using Privaseek.Domain.ValueObjects;
using SQLite;
using System.Linq;
using System.Threading;

namespace Privaseek.Infrastructure.Search;

public class AppUsageSearchService : IIndexedSearchService
{
    private readonly SQLiteAsyncConnection _db;

    public AppUsageSearchService(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
        _db.CreateTableAsync<AppUsageEntry>().Wait();
    }

    public async Task<IReadOnlyList<ResultItem>> QueryAsync(string query, CancellationToken ct = default)
    {
        var lower = query.ToLowerInvariant();
        var rows = await _db.Table<AppUsageEntry>()
            .Where(a => a.AppName.ToLower().Contains(lower)
                      || a.PackageName.ToLower().Contains(lower))
            .ToListAsync();

        return rows
            .Select(a => new ResultItem(
                Title: a.AppName,
                Subtitle: a.PackageName,
                Icon: "icon_app.png",
                Score: 1.0,
                Path: a.PackageName,
                Timestamp: a.LastTimeUsed,
                Type: ResultType.AppUsage))
            .ToList();
    }
}
