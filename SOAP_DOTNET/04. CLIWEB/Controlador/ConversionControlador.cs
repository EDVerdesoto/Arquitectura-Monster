using ReferenciaServicioConversion;

namespace ClienteWeb.Controlador
{
    public class ConversionControlador
    {
        private readonly Modelo.ServicioSesion _sesion;
        private readonly ServicioConversionClient _cliente;

        public ConversionControlador(Modelo.ServicioSesion sesion)
        {
            _sesion = sesion;
            _cliente = new ServicioConversionClient();
        }

        public async Task<(bool exito, double valorConvertido, string mensaje)> ConvertirAsync(
            Modelo.TipoConversion tipo, string unidadOrigen, string unidadDestino, double valor)
        {
            if (!_sesion.EstaAutenticado)
                return (false, 0, "Debe iniciar sesion para realizar conversiones.");

            try
            {
                var solicitud = new SolicitudConversion
                {
                    Token = _sesion.Token,
                    Valor = valor,
                    UnidadOrigen = unidadOrigen,
                    UnidadDestino = unidadDestino
                };

                RespuestaConversion resultado;

                switch (tipo)
                {
                    case Modelo.TipoConversion.Longitud:
                        {
                            var request = new ConvertirLongitudRequest(solicitud);
                            var response = await _cliente.ConvertirLongitudAsync(request);
                            resultado = response.ConvertirLongitudResult;
                            break;
                        }
                    case Modelo.TipoConversion.Temperatura:
                        {
                            var request = new ConvertirTemperaturaRequest(solicitud);
                            var response = await _cliente.ConvertirTemperaturaAsync(request);
                            resultado = response.ConvertirTemperaturaResult;
                            break;
                        }
                    case Modelo.TipoConversion.Masa:
                        {
                            var request = new ConvertirMasaRequest(solicitud);
                            var response = await _cliente.ConvertirMasaAsync(request);
                            resultado = response.ConvertirMasaResult;
                            break;
                        }
                    default:
                        return (false, 0, "Tipo de conversion no soportado.");
                }

                if (resultado.Exito)
                    return (true, resultado.ValorConvertido, string.Empty);

                return (false, 0, resultado.MensajeError);
            }
            catch (Exception ex)
            {
                return (false, 0, $"Error de conexion: {ex.Message}");
            }
        }
    }
}
