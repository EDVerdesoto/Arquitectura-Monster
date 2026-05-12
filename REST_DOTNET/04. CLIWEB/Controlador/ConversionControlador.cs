using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ClienteWeb.Modelo;

namespace ClienteWeb.Controlador
{
    public class ConversionControlador
    {
        private readonly ServicioSesion _sesion;
        private readonly HttpClient _httpClient;

        public ConversionControlador(ServicioSesion sesion, HttpClient httpClient)
        {
            _sesion = sesion;
            _httpClient = httpClient;
        }

        public async Task<(bool exito, double valorConvertido, string mensaje)> ConvertirAsync(
            TipoConversion tipo, string unidadOrigen, string unidadDestino, double valor)
        {
            if (!_sesion.EstaAutenticado)
                return (false, 0, "Debe iniciar sesion para realizar conversiones.");

            try
            {
                var solicitud = new { Valor = valor, UnidadOrigen = unidadOrigen, UnidadDestino = unidadDestino };
                var json = JsonSerializer.Serialize(solicitud);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, $"api/conversion/{tipo.ToString().ToLower()}")
                {
                    Content = content
                };
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _sesion.Token);

                var response = await _httpClient.SendAsync(request);
                var result = await response.Content.ReadFromJsonAsync<ConversionResponse>();

                if (result != null && result.Exito)
                    return (true, result.ValorConvertido, string.Empty);

                return (false, 0, result?.MensajeError ?? "Error desconocido");
            }
            catch (Exception ex)
            {
                return (false, 0, $"Error de conexion: {ex.Message}");
            }
        }

        private class ConversionResponse
        {
            public bool Exito { get; set; }
            public double ValorConvertido { get; set; }
            public string MensajeError { get; set; } = string.Empty;
        }
    }
}
