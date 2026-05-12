using ClienteMovil.Modelo;
using ClienteMovil.Controlador;
using ClienteMovil.Vista;

namespace ClienteMovil
{
    public partial class AppShell : Shell
    {
        private readonly ServicioSesion _sesion;

        public AppShell()
        {
            InitializeComponent();

            var services = IPlatformApplication.Current?.Services
                ?? throw new InvalidOperationException("Servicios no disponibles.");

            _sesion = services.GetRequiredService<ServicioSesion>();
            _sesion.PropertyChanged += OnSesionChanged;

            var loginPage = services.GetRequiredService<LoginPage>();
            var conversionPage = services.GetRequiredService<ConversionPage>();

            Items.Clear();

            var loginItem = new FlyoutItem
            {
                Title = "Login",
                Items =
                {
                    new ShellContent
                    {
                        Route = "login",
                        ContentTemplate = new DataTemplate(() => loginPage)
                    }
                }
            };

            var conversionItem = new FlyoutItem
            {
                Title = "Conversion",
                Items =
                {
                    new ShellContent
                    {
                        Route = "conversion",
                        ContentTemplate = new DataTemplate(() => conversionPage)
                    }
                }
            };

            Items.Add(loginItem);
            Items.Add(conversionItem);

            NavegarSegunSesion();
        }

        private async void NavegarSegunSesion()
        {
            if (_sesion.EstaAutenticado)
            {
                await GoToAsync("//conversion", true);
            }
            else
            {
                await GoToAsync("//login", true);
            }
        }

        private void OnSesionChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ServicioSesion.EstaAutenticado))
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    if (_sesion.EstaAutenticado)
                    {
                        await GoToAsync("//conversion", true);
                    }
                    else
                    {
                        await GoToAsync("//login", true);
                    }
                });
            }
        }
    }
}