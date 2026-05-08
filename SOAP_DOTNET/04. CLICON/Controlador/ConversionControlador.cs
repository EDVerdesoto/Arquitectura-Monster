using System;
using ClienteConsola.Modelo;
using ClienteConsola.Vista;
using ServicioConversionRef;

namespace ClienteConsola.Controlador
{
    public class ConversionControlador
    {
        private readonly ConversionVista _vista;
        private readonly ServicioConversionClient _cliente;

        public ConversionControlador(ConversionVista vista)
        {
            _vista = vista;
            _cliente = new ServicioConversionClient();
        }

        public void ConvertirLongitud()
        {
            EjecutarConversion(_vista.SolicitarConversionLongitud, _cliente.ConvertirLongitudAsync);
        }

        public void ConvertirTemperatura()
        {
            EjecutarConversion(_vista.SolicitarConversionTemperatura, _cliente.ConvertirTemperaturaAsync);
        }

        public void ConvertirMasa()
        {
            EjecutarConversion(_vista.SolicitarConversionMasa, _cliente.ConvertirMasaAsync);
        }

        public void ValidarCampoNumerico()
        {
            if (!SesionUsuario.EstaAutenticado)
            {
                Console.WriteLine("Debe iniciar sesion para usar esta funcion.");
                return;
            }

            string valorStr = _vista.SolicitarValidarNumerico();

            try
            {
                var solicitud = new SolicitudValidarNumerico
                {
                    Token = SesionUsuario.Token,
                    ValorStr = valorStr
                };

                var respuesta = _cliente.ValidarCampoNumericoAsync(solicitud).GetAwaiter().GetResult();
                _vista.MostrarResultadoValidacion(respuesta.Exito, respuesta.EsValido, respuesta.MensajeError);
            }
            catch (Exception ex)
            {
                _vista.MostrarResultadoValidacion(false, false, ex.Message);
            }
        }

        private void EjecutarConversion(
            Func<(double valor, string unidadOrigen, string unidadDestino)> solicitarDatos,
            Func<SolicitudConversion, System.Threading.Tasks.Task<RespuestaConversion>> operacion)
        {
            if (!SesionUsuario.EstaAutenticado)
            {
                Console.WriteLine("Debe iniciar sesion para usar esta funcion.");
                return;
            }

            var (valor, unidadOrigen, unidadDestino) = solicitarDatos();

            try
            {
                var solicitud = new SolicitudConversion
                {
                    Token = SesionUsuario.Token,
                    Valor = valor,
                    UnidadOrigen = unidadOrigen,
                    UnidadDestino = unidadDestino
                };

                var respuesta = operacion(solicitud).GetAwaiter().GetResult();
                _vista.MostrarResultadoConversion(respuesta.Exito, respuesta.ValorConvertido, respuesta.MensajeError);
            }
            catch (Exception ex)
            {
                _vista.MostrarResultadoConversion(false, 0, ex.Message);
            }
        }
    }
}
