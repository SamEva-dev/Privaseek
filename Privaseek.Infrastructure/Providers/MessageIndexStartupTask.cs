using Privaseek.Applications.Contracts;

namespace Privaseek.Infrastructure.Providers;

public class MessageIndexStartupTask : IStartupTask
{
    private readonly IMessageIndexer _indexer;
    public MessageIndexStartupTask(IMessageIndexer indexer)
        => _indexer = indexer;

    public Task ExecuteAsync(CancellationToken cancellationToken = default)
        => _indexer.IndexAsync(cancellationToken);
}
