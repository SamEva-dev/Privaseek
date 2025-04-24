using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Privaseek.App.ViewModels;
using Privaseek.Applications.Contracts;
using Privaseek.UI.Bootstrap;
using Privaseek.UI.ViewModels;
using Privaseek.UI.Views;

namespace Privaseek.UI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services
           
           .AddInfrastructure()    // met en place ISearchService → StubSearchService
           .AddSingleton<SearchViewModel>()
           .AddSingleton<HomePage>()
            .AddSingleton<SettingsViewModel>()
           .AddSingleton<SettingsPage>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            // Exécuter toutes les tâches de démarrage
            using var scope = app.Services.CreateScope();
            var startupTasks = scope.ServiceProvider.GetServices<IStartupTask>();

            _ = Task.Run(async () =>
            {
                using var scope = app.Services.CreateScope();
                var startupTasks = scope.ServiceProvider.GetServices<IStartupTask>();
                foreach (var task in startupTasks)
                    await task.ExecuteAsync();
            });


            return app;
        }
    }
}
