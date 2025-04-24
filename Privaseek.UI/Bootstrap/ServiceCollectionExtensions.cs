namespace Privaseek.UI.Bootstrap;

using Microsoft.Extensions.DependencyInjection;
using Privaseek.Applications.Contracts;
using Privaseek.Infrastructure.Providers;
using Privaseek.Infrastructure.Search;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "index.db");

        services.AddSingleton<IFileIndexer>(new FileIndexProvider(dbPath));
        services.AddSingleton<ISearchService>(sp =>
        {
            var fileIndexer = sp.GetRequiredService<IFileIndexer>();
            return new FileIndexerAdapter(fileIndexer);
        });

        services.AddSingleton<IStartupTask, FileIndexStartupTask>();

        return services;
    }
}
