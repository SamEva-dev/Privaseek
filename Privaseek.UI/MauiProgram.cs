using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
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
           .AddSingleton<HomePage>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
