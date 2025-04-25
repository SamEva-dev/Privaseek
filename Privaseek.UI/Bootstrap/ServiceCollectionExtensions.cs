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

        // File indexer (existant)
        services.AddSingleton<IFileIndexer>(new FileIndexProvider(dbPath));
        services.AddSingleton<ISearchService>(sp =>
        {
            var fileIndexer = sp.GetRequiredService<IFileIndexer>();
            return new FileIndexerAdapter(fileIndexer);
        });

        // Message indexer
        services.AddSingleton<IMessageIndexer>(sp => new MessageIndexProvider(dbPath));

        // App usage indexer
        services.AddSingleton<IAppUsageIndexer>(sp => new AppUsageProvider(dbPath));

        // Searchers individuels
    services.AddSingleton<IIndexedSearchService>(sp => new FileIndexerAdapter(sp.GetRequiredService<IFileIndexer>()));
        services.AddSingleton<IIndexedSearchService>(sp => new MessageSearchService(dbPath));
        services.AddSingleton<IIndexedSearchService>(sp => new AppUsageSearchService(dbPath));

        // Service de recherche composite
        services.AddSingleton<ISearchService, CompositeSearchService>();

        // Startup Tasks
        services.AddSingleton<IStartupTask, FileIndexStartupTask>();
        services.AddSingleton<IStartupTask, MessageIndexStartupTask>();
        services.AddSingleton<IStartupTask, AppUsageStartupTask>();

        return services;
    }
}
