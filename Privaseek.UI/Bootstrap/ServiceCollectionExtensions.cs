namespace Privaseek.UI.Bootstrap;

using Microsoft.Extensions.DependencyInjection;
using Privaseek.Applications.Contracts;
using Privaseek.Infrastructure.Search;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ISearchService, StubSearchService>();
        return services;
    }
}
