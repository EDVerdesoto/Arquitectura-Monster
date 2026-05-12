using ServicioConversionRef;

namespace ClienteMovil.Controlador
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
                        resultado = await _cliente.ConvertirLongitudAsync(solicitud);
                        break;
                    case Modelo.TipoConversion.Temperatura:
                        resultado = await _cliente.ConvertirTemperaturaAsync(solicitud);
                        break;
                    case Modelo.TipoConversion.Masa:
                        resultado = await _cliente.ConvertirMasaAsync(solicitud);
                        break;
                    default:
                        return (false, 0, "Tipo de conversion no soportado.");
                }

                if (resultado.Exito)
                    return (true, resultado.ValorConvertido, string.Empty);

                return (false, 0, resultado.MensajeError);
            }
            catch (Exception ex)
            {
                var detalle = ex.InnerException != null ? $" | Detalle: {ex.InnerException.Message}" : "";
                return (false, 0, $"Error de conexion: {ex.Message}{detalle}");
            }
        }
    }
}