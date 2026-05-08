using ServicioAutenticacionRef;

namespace ClienteMovil.Controlador
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

                var resultado = await _cliente.ValidarCredencialesAsync(solicitud);

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
            await Task.Delay(1);
            return (false, "Funcion proximamente disponible.");
        }

        public void CerrarSesion()
        {
            _sesion.ClearToken();
        }
    }
}