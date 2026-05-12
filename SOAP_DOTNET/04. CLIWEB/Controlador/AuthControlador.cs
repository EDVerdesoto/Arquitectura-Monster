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

        public void CerrarSesion()
        {
            _sesion.ClearToken();
        }
    }
}
