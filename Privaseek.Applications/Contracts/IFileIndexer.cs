namespace Privaseek.Applications.Contracts;

using Privaseek.Domain.ValueObjects;

public interface IFileIndexer
{
    Task IndexAsync(CancellationToken ct = default);
    Task<IReadOnlyList<ResultItem>> SearchAsync(string query, CancellationToken ct = default);
}
