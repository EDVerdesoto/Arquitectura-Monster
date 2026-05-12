using ClienteEscritorio.Controlador;
using ClienteEscritorio.Modelo;

namespace ClienteEscritorio.Vista
{
    public partial class ConversionPage : ContentPage
    {
        private readonly ServicioSesion _sesion;
        private readonly ConversionControlador _conversionControlador;

        public ConversionPage(ServicioSesion sesion, ConversionControlador conversionControlador)
        {
            InitializeComponent();
            _sesion = sesion;
            _conversionControlador = conversionControlador;

            _sesion.PropertyChanged += OnSesionChanged;

            InicializarTipoPicker();
            ActualizarVisibilidad();
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            ActualizarVisibilidad();
        }

        private void OnSesionChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ServicioSesion.EstaAutenticado) ||
                e.PropertyName == nameof(ServicioSesion.Token))
            {
                MainThread.BeginInvokeOnMainThread(ActualizarVisibilidad);
            }
        }

        private void ActualizarVisibilidad()
        {
            bool autenticado = _sesion.EstaAutenticado;
            NoAutenticadoFrame.IsVisible = !autenticado;
            ContenidoConversion.IsVisible = autenticado;
        }

        private void InicializarTipoPicker()
        {
            TipoPicker.ItemsSource = new List<string> { "Longitud", "Temperatura", "Masa" };
            TipoPicker.SelectedIndex = 0;
            ActualizarUnidades();
        }

        private TipoConversion ObtenerTipoSeleccionado()
        {
            return TipoPicker.SelectedIndex switch
            {
                0 => TipoConversion.Longitud,
                1 => TipoConversion.Temperatura,
                2 => TipoConversion.Masa,
                _ => TipoConversion.Longitud
            };
        }

        private void OnTipoChanged(object? sender, EventArgs e)
        {
            ActualizarUnidades();
        }

        private void ActualizarUnidades()
        {
            var tipo = ObtenerTipoSeleccionado();
            var unidades = UnidadesConversion.Unidades[tipo];

            OrigenPicker.ItemsSource = unidades.ToList();
            DestinoPicker.ItemsSource = unidades.ToList();

            OrigenPicker.SelectedIndex = 0;
            DestinoPicker.SelectedIndex = unidades.Count > 1 ? 1 : 0;
        }

        private async void OnConvertirClicked(object? sender, EventArgs e)
        {
            ConvertirButton.IsEnabled = false;

            try
            {
                var tipo = ObtenerTipoSeleccionado();
                string unidadOrigen = OrigenPicker.SelectedItem?.ToString() ?? string.Empty;
                string unidadDestino = DestinoPicker.SelectedItem?.ToString() ?? string.Empty;

                if (!double.TryParse(ValorEntry.Text, out double valor))
                {
                    MostrarMensaje("Ingrese un valor numerico valido.", false);
                    return;
                }

                var (exito, valorConvertido, mensaje) = await _conversionControlador.ConvertirAsync(
                    tipo, unidadOrigen, unidadDestino, valor);

                if (exito)
                {
                    ResultadoEntry.Text = valorConvertido.ToString("F4");
                    OcultarMensaje();
                }
                else
                {
                    ResultadoEntry.Text = string.Empty;
                    MostrarMensaje(mensaje, false);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error inesperado: {ex.Message}", false);
            }
            finally
            {
                ConvertirButton.IsEnabled = true;
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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}
