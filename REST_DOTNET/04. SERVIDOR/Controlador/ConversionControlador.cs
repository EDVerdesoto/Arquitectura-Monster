using ConversionesServerAPI.Modelo;
using Microsoft.AspNetCore.Mvc;

namespace ConversionesServerAPI.Controlador
{
    [Route("api/conversion")]
    [ApiController]
    public class ConversionControlador : ControllerBase
    {
        [HttpPost("longitud")]
        public ActionResult<ConversionResponse> ConvertirLongitud([FromBody] ConversionRequest solicitud)
        {
            var token = ExtraerToken();
            if (!AlmacenamientoAutenticacion.ValidarToken(token))
                return Ok(new ConversionResponse { Exito = false, MensajeError = "Token no autorizado" });

            try
            {
                if (string.IsNullOrEmpty(solicitud.UnidadOrigen) || string.IsNullOrEmpty(solicitud.UnidadDestino))
                    return Ok(new ConversionResponse { Exito = false, MensajeError = "Unidad no puede estar vacía" });

                double enMetros = solicitud.UnidadOrigen.ToLower() switch
                {
                    "centimetros" => solicitud.Valor / 100.0,
                    "metros" => solicitud.Valor,
                    "millas" => solicitud.Valor * 1609.34,
                    "yardas" => solicitud.Valor * 0.9144,
                    "pies" => solicitud.Valor * 0.3048,
                    _ => double.NaN
                };

                if (double.IsNaN(enMetros))
                    return Ok(new ConversionResponse { Exito = false, MensajeError = "Unidad de origen no válida" });

                double resultado = solicitud.UnidadDestino.ToLower() switch
                {
                    "centimetros" => enMetros * 100.0,
                    "metros" => enMetros,
                    "millas" => enMetros / 1609.34,
                    "yardas" => enMetros / 0.9144,
                    "pies" => enMetros / 0.3048,
                    _ => double.NaN
                };

                if (double.IsNaN(resultado))
                    return Ok(new ConversionResponse { Exito = false, MensajeError = "Unidad de destino no válida" });

                return Ok(new ConversionResponse { Exito = true, ValorConvertido = resultado });
            }
            catch (Exception ex)
            {
                return Ok(new ConversionResponse { Exito = false, MensajeError = ex.Message });
            }
        }

        [HttpPost("temperatura")]
        public ActionResult<ConversionResponse> ConvertirTemperatura([FromBody] ConversionRequest solicitud)
        {
            var token = ExtraerToken();
            if (!AlmacenamientoAutenticacion.ValidarToken(token))
                return Ok(new ConversionResponse { Exito = false, MensajeError = "Token no autorizado" });

            try
            {
                if (string.IsNullOrEmpty(solicitud.UnidadOrigen) || string.IsNullOrEmpty(solicitud.UnidadDestino))
                    return Ok(new ConversionResponse { Exito = false, MensajeError = "Unidad no puede estar vacía" });

                double enCelsius = solicitud.UnidadOrigen.ToLower() switch
                {
                    "celsius" => solicitud.Valor,
                    "fahrenheit" => (solicitud.Valor - 32) * 5.0 / 9.0,
                    "kelvin" => solicitud.Valor - 273.15,
                    "newton" => solicitud.Valor * 100.0 / 33.0,
                    "reaumur" => solicitud.Valor * 1.25,
                    _ => double.NaN
                };

                if (double.IsNaN(enCelsius))
                    return Ok(new ConversionResponse { Exito = false, MensajeError = "Unidad de origen no válida" });

                double resultado = solicitud.UnidadDestino.ToLower() switch
                {
                    "celsius" => enCelsius,
                    "fahrenheit" => (enCelsius * 9.0 / 5.0) + 32.0,
                    "kelvin" => enCelsius + 273.15,
                    "newton" => enCelsius * 33.0 / 100.0,
                    "reaumur" => enCelsius * 0.8,
                    _ => double.NaN
                };

                if (double.IsNaN(resultado))
                    return Ok(new ConversionResponse { Exito = false, MensajeError = "Unidad de destino no válida" });

                return Ok(new ConversionResponse { Exito = true, ValorConvertido = resultado });
            }
            catch (Exception ex)
            {
                return Ok(new ConversionResponse { Exito = false, MensajeError = ex.Message });
            }
        }

        [HttpPost("masa")]
        public ActionResult<ConversionResponse> ConvertirMasa([FromBody] ConversionRequest solicitud)
        {
            var token = ExtraerToken();
            if (!AlmacenamientoAutenticacion.ValidarToken(token))
                return Ok(new ConversionResponse { Exito = false, MensajeError = "Token no autorizado" });

            try
            {
                if (string.IsNullOrEmpty(solicitud.UnidadOrigen) || string.IsNullOrEmpty(solicitud.UnidadDestino))
                    return Ok(new ConversionResponse { Exito = false, MensajeError = "Unidad no puede estar vacía" });

                double enKilogramos = solicitud.UnidadOrigen.ToLower() switch
                {
                    "onzas" => solicitud.Valor * 0.0283495,
                    "libras" => solicitud.Valor * 0.453592,
                    "quintales" => solicitud.Valor * 100.0,
                    "gramos" => solicitud.Valor / 1000.0,
                    "kilogramos" => solicitud.Valor,
                    _ => double.NaN
                };

                if (double.IsNaN(enKilogramos))
                    return Ok(new ConversionResponse { Exito = false, MensajeError = "Unidad de origen no válida" });

                double resultado = solicitud.UnidadDestino.ToLower() switch
                {
                    "onzas" => enKilogramos / 0.0283495,
                    "libras" => enKilogramos / 0.453592,
                    "quintales" => enKilogramos / 100.0,
                    "gramos" => enKilogramos * 1000.0,
                    "kilogramos" => enKilogramos,
                    _ => double.NaN
                };

                if (double.IsNaN(resultado))
                    return Ok(new ConversionResponse { Exito = false, MensajeError = "Unidad de destino no válida" });

                return Ok(new ConversionResponse { Exito = true, ValorConvertido = resultado });
            }
            catch (Exception ex)
            {
                return Ok(new ConversionResponse { Exito = false, MensajeError = ex.Message });
            }
        }

        [HttpPost("validar-numerico")]
        public ActionResult<ValidarNumericoResponse> ValidarCampoNumerico([FromBody] ValidarNumericoRequest solicitud)
        {
            var token = ExtraerToken();
            if (!AlmacenamientoAutenticacion.ValidarToken(token))
                return Ok(new ValidarNumericoResponse { Exito = false, MensajeError = "Token no autorizado" });

            if (string.IsNullOrEmpty(solicitud.ValorStr))
                return Ok(new ValidarNumericoResponse { Exito = false, MensajeError = "Valor no puede estar vacío" });

            bool valido = double.TryParse(solicitud.ValorStr, out _);
            return Ok(new ValidarNumericoResponse { Exito = true, EsValido = valido });
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