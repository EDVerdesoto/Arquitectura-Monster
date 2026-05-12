using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ClienteWeb.Controlador
{
    public class AuthControlador
    {
        private readonly Modelo.ServicioSesion _sesion;
        private readonly HttpClient _httpClient;

        public AuthControlador(Modelo.ServicioSesion sesion, HttpClient httpClient)
        {
            _sesion = sesion;
            _httpClient = httpClient;
        }

        public async Task<(bool exito, string mensaje)> IniciarSesionAsync(string usuario, string clave)
        {
            try
            {
                var solicitud = new Modelo.LoginRequest
                {
                    Usuario = usuario,
                    Clave = clave
                };

                var response = await _httpClient.PostAsJsonAsync("api/auth/login", solicitud);
                var resultado = await response.Content.ReadFromJsonAsync<Modelo.LoginResponse>();

                if (resultado.Exito)
                {
                    _sesion.SetToken(resultado.TokenGenerado);
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", resultado.TokenGenerado);
                    return (true, "Sesion iniciada correctamente.");
                }

                return (false, resultado.MensajeError);
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexion: {ex.Message}");
            }
        }

        public void CerrarSesion()
        {
            _sesion.ClearToken();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}