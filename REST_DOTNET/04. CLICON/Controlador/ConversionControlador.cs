using System.Net.Http.Headers;
using System.Net.Http.Json;
using ClienteConsola.Modelo;
using ClienteConsola.Vista;

namespace ClienteConsola.Controlador
{
    public class ConversionControlador
    {
        private readonly ConversionVista _vista;
        private readonly HttpClient _httpClient;

        public ConversionControlador(ConversionVista vista, HttpClient httpClient)
        {
            _vista = vista;
            _httpClient = httpClient;
        }

        public void ConvertirLongitud()
        {
            EjecutarConversion(_vista.SolicitarConversionLongitud, "api/conversion/longitud");
        }

        public void ConvertirTemperatura()
        {
            EjecutarConversion(_vista.SolicitarConversionTemperatura, "api/conversion/temperatura");
        }

        public void ConvertirMasa()
        {
            EjecutarConversion(_vista.SolicitarConversionMasa, "api/conversion/masa");
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
                var solicitud = new ValidarNumericoRequest
                {
                    ValorStr = valorStr
                };

                var response = _httpClient.PostAsJsonAsync("api/conversion/validar-numerico", solicitud).GetAwaiter().GetResult();
                var resultado = response.Content.ReadFromJsonAsync<ValidarNumericoResponse>().GetAwaiter().GetResult();
                _vista.MostrarResultadoValidacion(resultado.Exito, resultado.EsValido, resultado.MensajeError);
            }
            catch (Exception ex)
            {
                _vista.MostrarResultadoValidacion(false, false, ex.Message);
            }
        }

        private void EjecutarConversion(
            Func<(double valor, string unidadOrigen, string unidadDestino)> solicitarDatos,
            string endpoint)
        {
            if (!SesionUsuario.EstaAutenticado)
            {
                Console.WriteLine("Debe iniciar sesion para usar esta funcion.");
                return;
            }

            var (valor, unidadOrigen, unidadDestino) = solicitarDatos();

            try
            {
                var solicitud = new ConversionRequest
                {
                    Valor = valor,
                    UnidadOrigen = unidadOrigen,
                    UnidadDestino = unidadDestino
                };

                var response = _httpClient.PostAsJsonAsync(endpoint, solicitud).GetAwaiter().GetResult();
                var resultado = response.Content.ReadFromJsonAsync<ConversionResponse>().GetAwaiter().GetResult();
                _vista.MostrarResultadoConversion(resultado.Exito, resultado.ValorConvertido, resultado.MensajeError);
            }
            catch (Exception ex)
            {
                _vista.MostrarResultadoConversion(false, 0, ex.Message);
            }
        }
    }
}