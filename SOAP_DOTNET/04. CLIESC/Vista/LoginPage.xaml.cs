using ClienteMovil.Controlador;
using ClienteMovil.Modelo;

namespace ClienteMovil.Vista
{
    public partial class LoginPage : ContentPage
    {
        private readonly ServicioSesion _sesion;
        private readonly AuthControlador _authControlador;

        public LoginPage(ServicioSesion sesion, AuthControlador authControlador)
        {
            InitializeComponent();
            _sesion = sesion;
            _authControlador = authControlador;
        }

        private async void OnLoginClicked(object? sender, EventArgs e)
        {
            LoginButton.IsEnabled = false;

            try
            {
                string usuario = UsuarioEntry.Text ?? string.Empty;
                string clave = ClaveEntry.Text ?? string.Empty;

                var (exito, mensaje) = await _authControlador.IniciarSesionAsync(usuario, clave);

                MostrarMensaje(mensaje, exito);

                if (exito)
                {
                    await Shell.Current.GoToAsync("//conversion");
                }
            }
            finally
            {
                LoginButton.IsEnabled = true;
            }
        }

        private void MostrarMensaje(string mensaje, bool exito)
        {
            MensajeFrame.IsVisible = true;
            MensajeLabel.Text = mensaje;

            if (exito)
            {
                MensajeFrame.BackgroundColor = Color.FromArgb("#D4EDDA");
                MensajeLabel.TextColor = Color.FromArgb("#155724");
            }
            else
            {
                MensajeFrame.BackgroundColor = Color.FromArgb("#F8D7DA");
                MensajeLabel.TextColor = Color.FromArgb("#721C24");
            }
        }

        private void OcultarMensaje()
        {
            MensajeFrame.IsVisible = false;
        }
    }
}