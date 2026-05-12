using ConversionesServerAPI.Modelo;
using Microsoft.AspNetCore.Mvc;

namespace ConversionesServerAPI.Controlador
{
    [Route("api/auth")]
    [ApiController]
    public class AuthControlador : ControllerBase
    {
        [HttpPost("login")]
        public ActionResult<LoginResponse> Login([FromBody] LoginRequest solicitud)
        {
            if (solicitud != null
                && !string.IsNullOrEmpty(solicitud.Usuario)
                && !string.IsNullOrEmpty(solicitud.Clave)
                && AlmacenamientoAutenticacion.Usuarios.ContainsKey(solicitud.Usuario)
                && AlmacenamientoAutenticacion.Usuarios[solicitud.Usuario] == solicitud.Clave)
            {
                string nuevoToken = Guid.NewGuid().ToString();
                AlmacenamientoAutenticacion.TokensActivos.Add(nuevoToken);
                return Ok(new LoginResponse { Exito = true, TokenGenerado = nuevoToken });
            }

            return Ok(new LoginResponse { Exito = false, MensajeError = "Credenciales incorrectas" });
        }

        private string? ExtraerToken()
        {
            var authHeader = HttpContext.Request.Headers.Authorization.ToString();
            if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return authHeader.Substring(7).Trim();
            return null;
        }
    }
}