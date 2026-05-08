using System;
using System.ServiceModel;
using ConversionesServerWCF.Modelo;

namespace ConversionesServerWCF.Controlador
{
    public class ServicioAutenticacion : IServicioAutenticacion
    {
        public RespuestaCredenciales ValidarCredenciales(SolicitudCredenciales solicitud)
        {
            if (solicitud != null && AlmacenamientoAutenticación.Usuarios.ContainsKey(solicitud.Usuario) && AlmacenamientoAutenticación.Usuarios[solicitud.Usuario] == solicitud.Clave)
            {
                string nuevoToken = Guid.NewGuid().ToString();
                AlmacenamientoAutenticación.TokensActivos.Add(nuevoToken);
                return new RespuestaCredenciales { Exito = true, TokenGenerado = nuevoToken, MensajeError = "" };
            }
            return new RespuestaCredenciales { Exito = false, MensajeError = "Credenciales incorrectas" };
        }

        public RespuestaBase CambiarContrasena(SolicitudCambioClave solicitud)
        {
            if (!AlmacenamientoAutenticación.ValidarToken(solicitud.Token)) return new RespuestaBase { Exito = false, MensajeError = "Token no autorizado" };

            if (AlmacenamientoAutenticación.Usuarios.ContainsKey(solicitud.Usuario) && AlmacenamientoAutenticación.Usuarios[solicitud.Usuario] == solicitud.ClaveAntigua)
            {
                AlmacenamientoAutenticación.Usuarios[solicitud.Usuario] = solicitud.ClaveNueva;
                return new RespuestaBase { Exito = true };
            }
            return new RespuestaBase { Exito = false, MensajeError = "Usuario o clave antigua incorrectos" };
        }

        public RespuestaRecuperarClave RecuperarContrasena(SolicitudRecuperarClave solicitud)
        {
            if (solicitud != null && AlmacenamientoAutenticación.Usuarios.ContainsKey(solicitud.Usuario))
            {
                return new RespuestaRecuperarClave { Exito = true, ClaveRecuperada = AlmacenamientoAutenticación.Usuarios[solicitud.Usuario] };
            }
            return new RespuestaRecuperarClave { Exito = false, MensajeError = "Usuario no encontrado" };
        }
    }
}
