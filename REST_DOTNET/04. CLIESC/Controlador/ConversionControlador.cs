using System.Net.Http.Json;

namespace ClienteMovil.Controlador
{
    public class ConversionControlador
    {
        private readonly Modelo.ServicioSesion _sesion;
        private readonly HttpClient _httpClient;

        public ConversionControlador(Modelo.ServicioSesion sesion, HttpClient httpClient)
        {
            _sesion = sesion;
            _httpClient = httpClient;
        }

        public async Task<(bool exito, double valorConvertido, string mensaje)> ConvertirAsync(
            Modelo.TipoConversion tipo, string unidadOrigen, string unidadDestino, double valor)
        {
            if (!_sesion.EstaAutenticado)
                return (false, 0, "Debe iniciar sesion para realizar conversiones.");

            try
            {
                var solicitud = new Modelo.ConversionRequest
                {
                    Valor = valor,
                    UnidadOrigen = unidadOrigen,
                    UnidadDestino = unidadDestino
                };

                string endpoint = tipo switch
                {
                    Modelo.TipoConversion.Longitud => "api/conversion/longitud",
                    Modelo.TipoConversion.Temperatura => "api/conversion/temperatura",
                    Modelo.TipoConversion.Masa => "api/conversion/masa",
                    _ => "api/conversion/longitud"
                };

                var response = await _httpClient.PostAsJsonAsync(endpoint, solicitud);
                var resultado = await response.Content.ReadFromJsonAsync<Modelo.ConversionResponse>();

                if (resultado.Exito)
                    return (true, resultado.ValorConvertido, string.Empty);

                return (false, 0, resultado.MensajeError);
            }
            catch (Exception ex)
            {
                return (false, 0, $"Error de conexion: {ex.Message}");
            }
        }
    }
}