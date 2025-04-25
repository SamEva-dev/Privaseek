using Privaseek.Applications.Contracts;
using Privaseek.Domain.Enums;
using Privaseek.Domain.ValueObjects;
using System.Linq;
using System.Threading;

namespace Privaseek.Infrastructure.Search;

public class CompositeSearchService : ISearchService
{
    private readonly IEnumerable<IIndexedSearchService> _searchers;

    public CompositeSearchService(IEnumerable<IIndexedSearchService> searchers)
        => _searchers = searchers;

    public async Task<IReadOnlyList<ResultItem>> QueryAsync(string query, CancellationToken ct = default)
    {
        var tasks = _searchers.Select(s => s.QueryAsync(query, ct));
        var results = (await Task.WhenAll(tasks))
                      .SelectMany(r => r);

        var weighted = results
            .Select(r =>
            {
                // pondération par type
                var typeWeight = r.Type switch
                {
                    ResultType.File => 1.0,
                    ResultType.Message => 0.9,
                    ResultType.AppUsage => 0.8,
                    _ => 1.0
                };
                // score ajusté
                var newScore = r.Score * typeWeight;
                return r with { Score = newScore };
            });

        // tri descendant
        return weighted
            .OrderByDescending(r => r.Score)
            .ToList();
    }

}
