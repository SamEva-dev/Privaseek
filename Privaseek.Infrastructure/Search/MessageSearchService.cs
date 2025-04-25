using Privaseek.Applications.Contracts;
using Privaseek.Domain.Entities;
using Privaseek.Domain.Enums;
using Privaseek.Domain.ValueObjects;
using SQLite;

namespace Privaseek.Infrastructure.Search;

public class MessageSearchService : IIndexedSearchService
{
    private readonly SQLiteAsyncConnection _db;

    public MessageSearchService(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
        _db.CreateTableAsync<IndexedMessage>().Wait();
    }

    public async Task<IReadOnlyList<ResultItem>> QueryAsync(string query, CancellationToken ct = default)
    {
        var lower = query.ToLowerInvariant();
        var rows = await _db.Table<IndexedMessage>()
            .Where(m => m.Body.ToLower().Contains(lower)
                     || m.Sender.ToLower().Contains(lower))
            .ToListAsync();

        return rows
            .Select(m => new ResultItem(
                Title: m.Sender,
                Subtitle: m.Body,
                Icon: "icon_message.png",
                Score: 1.0,               // placeholder scoring
                Path: m.Id.ToString(),
                Timestamp: m.SentDate,
                Type: ResultType.Message))
            .ToList();
    }
}
