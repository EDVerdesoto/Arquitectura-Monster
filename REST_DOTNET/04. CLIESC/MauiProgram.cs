using ClienteMovil.Modelo;
using ClienteMovil.Controlador;
using ClienteMovil.Vista;
using Microsoft.Extensions.Logging;

namespace ClienteMovil
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<ServicioSesion>();

            builder.Services.AddHttpClient<AuthControlador>(client =>
                client.BaseAddress = new Uri("https://localhost:5001/"));
            builder.Services.AddHttpClient<ConversionControlador>(client =>
                client.BaseAddress = new Uri("https://localhost:5001/"));

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ConversionPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}