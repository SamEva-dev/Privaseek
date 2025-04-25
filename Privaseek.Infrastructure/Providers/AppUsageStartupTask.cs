using Privaseek.Applications.Contracts;

namespace Privaseek.Infrastructure.Providers;

public class AppUsageStartupTask : IStartupTask
{
    private readonly IAppUsageIndexer _indexer;
    public AppUsageStartupTask(IAppUsageIndexer indexer)
        => _indexer = indexer;

    public Task ExecuteAsync(CancellationToken cancellationToken = default)
        => _indexer.IndexAsync(cancellationToken);
}
