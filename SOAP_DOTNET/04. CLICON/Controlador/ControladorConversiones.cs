using System;
using ServicioAutenticacionRef;
using ServicioConversionRef;

namespace ClienteConsola.Controlador
{
    public class ControladorConversiones
    {
        private readonly ServicioAutenticacionClient _clienteAuth;
        private readonly ServicioConversionClient _clienteConv;
        private string? _token;

        public ControladorConversiones()
        {
            _clienteAuth = new ServicioAutenticacionClient();
            _clienteConv = new ServicioConversionClient();
        }

        public bool iniciarSesion(string usuario, string clave)
        {
            try
            {
                var solicitud = new SolicitudCredenciales
                {
                    Usuario = usuario,
                    Clave = clave
                };

                var respuesta = _clienteAuth.ValidarCredencialesAsync(solicitud).GetAwaiter().GetResult();

                if (respuesta.Exito)
                {
                    _token = respuesta.TokenGenerado;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public double convertirLongitud(double valor, string origen, string destino)
        {
            return EjecutarConversion(_clienteConv.ConvertirLongitudAsync, valor, origen, destino);
        }

        public double convertirTemperatura(double valor, string origen, string destino)
        {
            return EjecutarConversion(_clienteConv.ConvertirTemperaturaAsync, valor, origen, destino);
        }

        public double convertirMasa(double valor, string origen, string destino)
        {
            return EjecutarConversion(_clienteConv.ConvertirMasaAsync, valor, origen, destino);
        }

        public bool validarCampoNumerico(string valorStr)
        {
            if (string.IsNullOrEmpty(_token))
                throw new InvalidOperationException("No hay sesion activa.");

            var solicitud = new SolicitudValidarNumerico
            {
                Token = _token,
                ValorStr = valorStr
            };

            var respuesta = _clienteConv.ValidarCampoNumericoAsync(solicitud).GetAwaiter().GetResult();

            if (!respuesta.Exito)
                throw new Exception(respuesta.MensajeError);

            return respuesta.EsValido;
        }

        private double EjecutarConversion(
            Func<SolicitudConversion, System.Threading.Tasks.Task<RespuestaConversion>> operacion,
            double valor, string origen, string destino)
        {
            if (string.IsNullOrEmpty(_token))
                throw new InvalidOperationException("No hay sesion activa.");

            var solicitud = new SolicitudConversion
            {
                Token = _token,
                Valor = valor,
                UnidadOrigen = origen,
                UnidadDestino = destino
            };

            var respuesta = operacion(solicitud).GetAwaiter().GetResult();

            if (!respuesta.Exito)
                throw new Exception(respuesta.MensajeError);

            return respuesta.ValorConvertido;
        }
    }
}
