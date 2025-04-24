using Privaseek.Applications.Contracts;
using Privaseek.Domain.Entities;
using Privaseek.Domain.ValueObjects;
using SQLite;

namespace Privaseek.Infrastructure.Providers;

public class FileIndexProvider : IFileIndexer
{
    private readonly SQLiteAsyncConnection _db;

    public FileIndexProvider(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
        _db.CreateTableAsync<IndexedFile>().Wait();
    }

    public async Task IndexAsync(CancellationToken ct = default)
    {
        var files = await GetAllAccessibleFilesAsync(ct);
        var indexed = files.Select(file => new IndexedFile
        {
            Title = Path.GetFileName(file),
            Path = file,
            Extension = Path.GetExtension(file),
            CreatedAt = File.GetCreationTimeUtc(file)
        }).ToList();

        await _db.DeleteAllAsync<IndexedFile>();
        await _db.InsertAllAsync(indexed);
    }

    public async Task<IReadOnlyList<ResultItem>> SearchAsync(string query, CancellationToken ct = default)
    {
        var lowerQuery = query.ToLowerInvariant();
        var matching = await _db.Table<IndexedFile>()
            .Where(f => f.Title.ToLower().Contains(lowerQuery) || f.Path.ToLower().Contains(lowerQuery))
            .ToListAsync();

        return matching
            .Select(f => new ResultItem(
                f.Title,
                f.Path,
                GetIconFromExtension(f.Extension),
                1.0,
                f.Path))
            .ToList();
    }

    private string GetIconFromExtension(string ext)
        => ext.ToLowerInvariant() switch
        {
            ".pdf" => "file_pdf.png",
            ".docx" => "file_doc.png",
            ".jpg" or ".png" => "image.png",
            _ => "file_generic.png"
        };

    private async Task<List<string>> GetAllAccessibleFilesAsync(CancellationToken ct)
    {
#if ANDROID
        var path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
        return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).ToList();
#elif IOS
        var docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        return Directory.GetFiles(docs, "*.*", SearchOption.AllDirectories).ToList();
#else
        return new();
#endif
    }
}
