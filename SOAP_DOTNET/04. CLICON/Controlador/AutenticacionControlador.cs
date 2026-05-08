using System;
using ClienteConsola.Modelo;
using ClienteConsola.Vista;
using ServicioAutenticacionRef;

namespace ClienteConsola.Controlador
{
    public class AutenticacionControlador
    {
        private readonly AutenticacionVista _vista;
        private readonly ServicioAutenticacionClient _cliente;

        public AutenticacionControlador(AutenticacionVista vista)
        {
            _vista = vista;
            _cliente = new ServicioAutenticacionClient();
        }

        public void IniciarSesion()
        {
            var (usuario, clave) = _vista.SolicitarCredenciales();

            try
            {
                var solicitud = new SolicitudCredenciales
                {
                    Usuario = usuario,
                    Clave = clave
                };

                var respuesta = _cliente.ValidarCredencialesAsync(solicitud).GetAwaiter().GetResult();

                if (respuesta.Exito)
                {
                    SesionUsuario.Token = respuesta.TokenGenerado;
                    _vista.MostrarResultadoLogin(true, respuesta.TokenGenerado, string.Empty);
                }
                else
                {
                    _vista.MostrarResultadoLogin(false, string.Empty, respuesta.MensajeError);
                }
            }
            catch (Exception ex)
            {
                _vista.MostrarResultadoLogin(false, string.Empty, ex.Message);
            }
        }

        public void CambiarContrasena()
        {
            if (!SesionUsuario.EstaAutenticado)
            {
                Console.WriteLine("Debe iniciar sesion para cambiar la contrasena.");
                return;
            }

            var (usuario, claveAntigua, claveNueva) = _vista.SolicitarCambioClave();

            try
            {
                var solicitud = new SolicitudCambioClave
                {
                    Token = SesionUsuario.Token,
                    Usuario = usuario,
                    ClaveAntigua = claveAntigua,
                    ClaveNueva = claveNueva
                };

                var respuesta = _cliente.CambiarContrasenaAsync(solicitud).GetAwaiter().GetResult();
                _vista.MostrarResultadoCambioClave(respuesta.Exito, respuesta.MensajeError);
            }
            catch (Exception ex)
            {
                _vista.MostrarResultadoCambioClave(false, ex.Message);
            }
        }

        public void RecuperarContrasena()
        {
            string usuario = _vista.SolicitarRecuperarClave();

            try
            {
                var solicitud = new SolicitudRecuperarClave
                {
                    Usuario = usuario
                };

                var respuesta = _cliente.RecuperarContrasenaAsync(solicitud).GetAwaiter().GetResult();
                _vista.MostrarResultadoRecuperarClave(respuesta.Exito, respuesta.ClaveRecuperada, respuesta.MensajeError);
            }
            catch (Exception ex)
            {
                _vista.MostrarResultadoRecuperarClave(false, string.Empty, ex.Message);
            }
        }

        public void CerrarSesion()
        {
            SesionUsuario.Token = null;
            Console.WriteLine("Sesion cerrada correctamente.");
        }
    }
}
