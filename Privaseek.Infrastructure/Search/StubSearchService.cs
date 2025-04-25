namespace Privaseek.Infrastructure.Search;

using Privaseek.Applications.Contracts;
using Privaseek.Domain.ValueObjects;

public class StubSearchService : ISearchService
{
    public Task<IReadOnlyList<ResultItem>> QueryAsync(string query, CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<ResultItem>>(new[]
        {
            new ResultItem("Exemple.pdf", "Docs/Exemple.pdf", "file_pdf.png", 0.93, "/storage/.../Exemple.pdf", DateTime.Now, Domain.Enums.ResultType.File)
        });
}
