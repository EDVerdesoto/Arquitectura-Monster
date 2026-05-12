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

            builder.Services.AddTransient(sp =>
            {
                var client = new HttpClient { BaseAddress = new Uri("https://server_rest.dr00p3r.top/") };
                return new AuthControlador(sp.GetRequiredService<ServicioSesion>(), client);
            });

            builder.Services.AddTransient(sp =>
            {
                var client = new HttpClient { BaseAddress = new Uri("https://server_rest.dr00p3r.top/") };
                return new ConversionControlador(sp.GetRequiredService<ServicioSesion>(), client);
            });

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ConversionPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
