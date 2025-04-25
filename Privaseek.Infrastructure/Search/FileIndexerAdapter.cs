using Privaseek.Applications.Contracts;
using Privaseek.Domain.ValueObjects;

namespace Privaseek.Infrastructure.Search;

public class FileIndexerAdapter : IIndexedSearchService
{
    private readonly IFileIndexer _indexer;

    public FileIndexerAdapter(IFileIndexer indexer) => _indexer = indexer;

    public Task<IReadOnlyList<ResultItem>> QueryAsync(string query, CancellationToken ct = default)
        => _indexer.SearchAsync(query, ct);
}
