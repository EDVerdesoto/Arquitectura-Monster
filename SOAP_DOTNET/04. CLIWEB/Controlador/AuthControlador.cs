using ReferenciaServicio;

namespace ClienteWeb.Controlador
{
    public class AuthControlador
    {
        private readonly Modelo.ServicioSesion _sesion;
        private readonly ServicioAutenticacionClient _cliente;

        public AuthControlador(Modelo.ServicioSesion sesion)
        {
            _sesion = sesion;
            _cliente = new ServicioAutenticacionClient();
        }

        public async Task<(bool exito, string mensaje)> IniciarSesionAsync(string usuario, string clave)
        {
            try
            {
                var solicitud = new SolicitudCredenciales
                {
                    Usuario = usuario,
                    Clave = clave
                };

                var request = new ValidarCredencialesRequest(solicitud);
                var response = await _cliente.ValidarCredencialesAsync(request);
                var resultado = response.ValidarCredencialesResult;

                if (resultado.Exito)
                {
                    _sesion.SetToken(resultado.TokenGenerado);
                    return (true, "Sesion iniciada correctamente.");
                }

                return (false, resultado.MensajeError);
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexion: {ex.Message}");
            }
        }

        public async Task<(bool exito, string mensaje)> RecuperarContrasenaAsync(string usuario)
        {
            // Placeholder: UI shows this as "proximamente disponible"
            // Uncomment below to enable real WCF call:
            /*
            try
            {
                var solicitud = new SolicitudRecuperarClave { Usuario = usuario };
                var request = new RecuperarContrasenaRequest(solicitud);
                var response = await _cliente.RecuperarContrasenaAsync(request);
                var resultado = response.RecuperarContrasenaResult;
                return (resultado.Exito, resultado.Exito ? $"Clave recuperada: {resultado.ClaveRecuperada}" : resultado.MensajeError);
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexion: {ex.Message}");
            }
            */
            await Task.Delay(1);
            return (false, "Funcion proximamente disponible.");
        }

        public void CerrarSesion()
        {
            _sesion.ClearToken();
        }
    }
}
