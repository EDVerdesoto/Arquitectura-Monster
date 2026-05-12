using System.Net.Http.Headers;
using System.Net.Http.Json;
using ClienteConsola.Modelo;
using ClienteConsola.Vista;

namespace ClienteConsola.Controlador
{
    public class AutenticacionControlador
    {
        private readonly AutenticacionVista _vista;
        private readonly HttpClient _httpClient;

        public AutenticacionControlador(AutenticacionVista vista, HttpClient httpClient)
        {
            _vista = vista;
            _httpClient = httpClient;
        }

        public void IniciarSesion()
        {
            var (usuario, clave) = _vista.SolicitarCredenciales();

            try
            {
                var solicitud = new LoginRequest
                {
                    Usuario = usuario,
                    Clave = clave
                };

                var response = _httpClient.PostAsJsonAsync("api/auth/login", solicitud).GetAwaiter().GetResult();
                var resultado = response.Content.ReadFromJsonAsync<LoginResponse>().GetAwaiter().GetResult();

                if (resultado.Exito)
                {
                    SesionUsuario.Token = resultado.TokenGenerado;
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SesionUsuario.Token);
                    _vista.MostrarResultadoLogin(true, resultado.TokenGenerado, string.Empty);
                }
                else
                {
                    _vista.MostrarResultadoLogin(false, string.Empty, resultado.MensajeError);
                }
            }
            catch (Exception ex)
            {
                _vista.MostrarResultadoLogin(false, string.Empty, ex.Message);
            }
        }

        public void CerrarSesion()
        {
            SesionUsuario.Token = null;
            _httpClient.DefaultRequestHeaders.Authorization = null;
            Console.WriteLine("Sesion cerrada correctamente.");
        }
    }
}