using Privaseek.Applications.Contracts;
using Privaseek.Domain.Entities;
using SQLite;

namespace Privaseek.Infrastructure.Providers;

public class MessageIndexProvider : IMessageIndexer
{
    private readonly SQLiteAsyncConnection _db;

    public MessageIndexProvider(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
        _db.CreateTableAsync<IndexedMessage>().Wait();
    }

    public async Task IndexAsync(CancellationToken ct = default)
    {
#if ANDROID
        var uri = Android.Net.Uri.Parse("content://sms/inbox");
        var cursor = Android.App.Application.Context.ContentResolver.Query(
            uri, null, null, null, null);

        var messages = new List<IndexedMessage>();
        while (cursor != null && cursor.MoveToNext())
        {
            var body = cursor.GetString(cursor.GetColumnIndex("body")) ?? "";
            var addr = cursor.GetString(cursor.GetColumnIndex("address")) ?? "";
            var dateMs = cursor.GetLong(cursor.GetColumnIndex("date"));
            var date = DateTimeOffset.FromUnixTimeMilliseconds(dateMs).UtcDateTime;

            messages.Add(new IndexedMessage
            {
                Sender = addr,
                Body = body,
                SentDate = date
            });
        }
        cursor?.Close();
#else
        var messages = new List<IndexedMessage>();
#endif

        // Réécriture complète
        await _db.DeleteAllAsync<IndexedMessage>();
        if (messages.Count > 0)
            await _db.InsertAllAsync(messages);
    }
}
