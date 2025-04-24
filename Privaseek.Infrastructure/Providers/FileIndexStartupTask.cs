using Privaseek.Applications.Contracts;

namespace Privaseek.Infrastructure.Providers;

public class FileIndexStartupTask : IStartupTask
{
    private readonly IFileIndexer _fileIndexer;

    public FileIndexStartupTask(IFileIndexer fileIndexer)
    {
        _fileIndexer = fileIndexer;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _fileIndexer.IndexAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            // log ou ignorer si pas critique
            System.Diagnostics.Debug.WriteLine($"[StartupTask] Indexation échouée: {ex.Message}");
        }
    }
}
