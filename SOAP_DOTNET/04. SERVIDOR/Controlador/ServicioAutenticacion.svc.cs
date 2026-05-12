using System;
using System.ServiceModel;
using ConversionesServerWCF.Modelo;

namespace ConversionesServerWCF.Controlador
{
    public class ServicioAutenticacion : IServicioAutenticacion
    {
        public RespuestaCredenciales ValidarCredenciales(SolicitudCredenciales solicitud)
        {
            if (solicitud != null
                && !string.IsNullOrEmpty(solicitud.Usuario)
                && !string.IsNullOrEmpty(solicitud.Clave)
                && AlmacenamientoAutenticación.Usuarios.ContainsKey(solicitud.Usuario)
                && AlmacenamientoAutenticación.Usuarios[solicitud.Usuario] == solicitud.Clave)
            {
                string nuevoToken = Guid.NewGuid().ToString();
                AlmacenamientoAutenticación.TokensActivos.Add(nuevoToken);
                return new RespuestaCredenciales { Exito = true, TokenGenerado = nuevoToken, MensajeError = "" };
            }
            return new RespuestaCredenciales { Exito = false, MensajeError = "Credenciales incorrectas" };
        }


    }
}
