using Privaseek.Domain.ValueObjects;


namespace Privaseek.Applications.Contracts;

public interface ISearchService
{
    Task<IReadOnlyList<ResultItem>> QueryAsync(string query, CancellationToken ct = default);
}