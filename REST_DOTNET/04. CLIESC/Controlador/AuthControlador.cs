using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ClienteEscritorio.Modelo;

namespace ClienteEscritorio.Controlador
{
    public class AuthControlador
    {
        private readonly ServicioSesion _sesion;
        private readonly HttpClient _httpClient;

        public AuthControlador(ServicioSesion sesion, HttpClient httpClient)
        {
            _sesion = sesion;
            _httpClient = httpClient;
        }

        public async Task<(bool exito, string mensaje)> IniciarSesionAsync(string usuario, string clave)
        {
            try
            {
                var solicitud = new { Usuario = usuario, Clave = clave };
                var json = JsonSerializer.Serialize(solicitud);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/auth/login", content);
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                if (result != null && result.Exito)
                {
                    _sesion.SetToken(result.TokenGenerado);
                    return (true, "Sesion iniciada correctamente.");
                }

                return (false, result?.MensajeError ?? "Error desconocido");
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexion: {ex.Message}");
            }
        }

        public void CerrarSesion()
        {
            _sesion.ClearToken();
        }

        private class LoginResponse
        {
            public bool Exito { get; set; }
            public string TokenGenerado { get; set; } = string.Empty;
            public string MensajeError { get; set; } = string.Empty;
        }
    }
}
