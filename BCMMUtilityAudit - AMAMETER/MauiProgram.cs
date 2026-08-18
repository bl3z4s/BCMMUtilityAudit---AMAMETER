using BCMMUtilityAudit___AMAMETER.Services;
using BCMMUtilityAudit___AMAMETER.Views;
using Microsoft.Extensions.Logging;
using Plugin.Maui.OCR;

namespace BCMMUtilityAudit___AMAMETER
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseOcr()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register Views & Services for Dependency Injection
            builder.Services.AddTransient<CameraPage>();
            // Add this inside your CreateMauiApp method where services are registered:
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddTransient<CameraPage>();
            builder.Services.AddTransient<HistoryPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}