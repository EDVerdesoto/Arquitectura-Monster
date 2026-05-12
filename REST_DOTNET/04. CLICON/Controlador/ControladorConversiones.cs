using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ClienteConsola.Controlador
{
    public class ControladorConversiones
    {
        private readonly HttpClient _httpClient;
        private string? _token;

        public ControladorConversiones()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://server_rest.dr00p3r.top/")
            };
        }

        public bool iniciarSesion(string usuario, string clave)
        {
            try
            {
                var solicitud = new { Usuario = usuario, Clave = clave };
                var json = JsonSerializer.Serialize(solicitud);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync("api/auth/login", content).GetAwaiter().GetResult();
                var result = response.Content.ReadFromJsonAsync<LoginResponse>().GetAwaiter().GetResult();

                if (result != null && result.Exito)
                {
                    _token = result.TokenGenerado;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public double convertirLongitud(double valor, string origen, string destino)
        {
            return EjecutarConversion("api/conversion/longitud", valor, origen, destino);
        }

        public double convertirTemperatura(double valor, string origen, string destino)
        {
            return EjecutarConversion("api/conversion/temperatura", valor, origen, destino);
        }

        public double convertirMasa(double valor, string origen, string destino)
        {
            return EjecutarConversion("api/conversion/masa", valor, origen, destino);
        }

        public bool validarCampoNumerico(string valorStr)
        {
            if (string.IsNullOrEmpty(_token))
                throw new InvalidOperationException("No hay sesion activa.");

            var solicitud = new { ValorStr = valorStr };
            var json = JsonSerializer.Serialize(solicitud);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "api/conversion/validar-numerico")
            {
                Content = content
            };
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

            var response = _httpClient.SendAsync(request).GetAwaiter().GetResult();
            var result = response.Content.ReadFromJsonAsync<ValidarNumericoResponse>().GetAwaiter().GetResult();

            if (result == null || !result.Exito)
                throw new Exception(result?.MensajeError ?? "Error desconocido");

            return result.EsValido;
        }

        private double EjecutarConversion(string endpoint, double valor, string origen, string destino)
        {
            if (string.IsNullOrEmpty(_token))
                throw new InvalidOperationException("No hay sesion activa.");

            var solicitud = new { Valor = valor, UnidadOrigen = origen, UnidadDestino = destino };
            var json = JsonSerializer.Serialize(solicitud);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = content
            };
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

            var response = _httpClient.SendAsync(request).GetAwaiter().GetResult();
            var result = response.Content.ReadFromJsonAsync<ConversionResponse>().GetAwaiter().GetResult();

            if (result == null || !result.Exito)
                throw new Exception(result?.MensajeError ?? "Error desconocido");

            return result.ValorConvertido;
        }

        private class LoginResponse
        {
            public bool Exito { get; set; }
            public string TokenGenerado { get; set; } = string.Empty;
            public string MensajeError { get; set; } = string.Empty;
        }

        private class ConversionResponse
        {
            public bool Exito { get; set; }
            public double ValorConvertido { get; set; }
            public string MensajeError { get; set; } = string.Empty;
        }

        private class ValidarNumericoResponse
        {
            public bool Exito { get; set; }
            public bool EsValido { get; set; }
            public string MensajeError { get; set; } = string.Empty;
        }
    }
}
