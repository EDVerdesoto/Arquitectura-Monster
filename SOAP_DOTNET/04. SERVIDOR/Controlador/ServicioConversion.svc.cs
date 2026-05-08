using System;
using System.ServiceModel;
using ConversionesServerWCF.Modelo;

namespace ConversionesServerWCF.Controlador
{
    public class ServicioConversion : IServicioConversion
    {
        public RespuestaConversion ConvertirLongitud(SolicitudConversion solicitud)
        {
            if (!AlmacenamientoAutenticación.ValidarToken(solicitud.Token)) return new RespuestaConversion { Exito = false, MensajeError = "Token no autorizado" };

            try
            {
                double enMetros = solicitud.Valor;
                switch (solicitud.UnidadOrigen.ToLower())
                {
                    case "centimetros": enMetros = solicitud.Valor / 100.0; break;
                    case "metros": enMetros = solicitud.Valor; break;
                    case "millas": enMetros = solicitud.Valor * 1609.34; break;
                    case "yardas": enMetros = solicitud.Valor * 0.9144; break;
                    case "pies": enMetros = solicitud.Valor * 0.3048; break;
                    default: return new RespuestaConversion { Exito = false, MensajeError = "Unidad de origen no válida" };
                }

                double resultado = 0;
                switch (solicitud.UnidadDestino.ToLower())
                {
                    case "centimetros": resultado = enMetros * 100.0; break;
                    case "metros": resultado = enMetros; break;
                    case "millas": resultado = enMetros / 1609.34; break;
                    case "yardas": resultado = enMetros / 0.9144; break;
                    case "pies": resultado = enMetros / 0.3048; break;
                    default: return new RespuestaConversion { Exito = false, MensajeError = "Unidad de destino no válida" };
                }
                return new RespuestaConversion { Exito = true, ValorConvertido = resultado };
            }
            catch (Exception ex)
            {
                return new RespuestaConversion { Exito = false, MensajeError = ex.Message };
            }
        }

        public RespuestaConversion ConvertirTemperatura(SolicitudConversion solicitud)
        {
            if (!AlmacenamientoAutenticación.ValidarToken(solicitud.Token)) return new RespuestaConversion { Exito = false, MensajeError = "Token no autorizado" };

            try
            {
                double enCelsius = solicitud.Valor;
                switch (solicitud.UnidadOrigen.ToLower())
                {
                    case "celsius": enCelsius = solicitud.Valor; break;
                    case "fahrenheit": enCelsius = (solicitud.Valor - 32) * 5.0 / 9.0; break;
                    case "kelvin": enCelsius = solicitud.Valor - 273.15; break;
                    case "newton": enCelsius = solicitud.Valor * 100.0 / 33.0; break;
                    case "reaumur": enCelsius = solicitud.Valor * 1.25; break;
                    default: return new RespuestaConversion { Exito = false, MensajeError = "Unidad de origen no válida" };
                }

                double resultado = 0;
                switch (solicitud.UnidadDestino.ToLower())
                {
                    case "celsius": resultado = enCelsius; break;
                    case "fahrenheit": resultado = (enCelsius * 9.0 / 5.0) + 32.0; break;
                    case "kelvin": resultado = enCelsius + 273.15; break;
                    case "newton": resultado = enCelsius * 33.0 / 100.0; break;
                    case "reaumur": resultado = enCelsius * 0.8; break;
                    default: return new RespuestaConversion { Exito = false, MensajeError = "Unidad de destino no válida" };
                }
                return new RespuestaConversion { Exito = true, ValorConvertido = resultado };
            }
            catch (Exception ex)
            {
                return new RespuestaConversion { Exito = false, MensajeError = ex.Message };
            }
        }

        public RespuestaConversion ConvertirMasa(SolicitudConversion solicitud)
        {
            if (!AlmacenamientoAutenticación.ValidarToken(solicitud.Token)) return new RespuestaConversion { Exito = false, MensajeError = "Token no autorizado" };

            try
            {
                double enKilogramos = solicitud.Valor;
                switch (solicitud.UnidadOrigen.ToLower())
                {
                    case "onzas": enKilogramos = solicitud.Valor * 0.0283495; break;
                    case "libras": enKilogramos = solicitud.Valor * 0.453592; break;
                    case "quintales": enKilogramos = solicitud.Valor * 100.0; break;
                    case "gramos": enKilogramos = solicitud.Valor / 1000.0; break;
                    case "kilogramos": enKilogramos = solicitud.Valor; break;
                    default: return new RespuestaConversion { Exito = false, MensajeError = "Unidad de origen no válida" };
                }

                double resultado = 0;
                switch (solicitud.UnidadDestino.ToLower())
                {
                    case "onzas": resultado = enKilogramos / 0.0283495; break;
                    case "libras": resultado = enKilogramos / 0.453592; break;
                    case "quintales": resultado = enKilogramos / 100.0; break;
                    case "gramos": resultado = enKilogramos * 1000.0; break;
                    case "kilogramos": resultado = enKilogramos; break;
                    default: return new RespuestaConversion { Exito = false, MensajeError = "Unidad de destino no válida" };
                }
                return new RespuestaConversion { Exito = true, ValorConvertido = resultado };
            }
            catch (Exception ex)
            {
                return new RespuestaConversion { Exito = false, MensajeError = ex.Message };
            }
        }

        public RespuestaValidarNumerico ValidarCampoNumerico(SolicitudValidarNumerico solicitud)
        {
            if (!AlmacenamientoAutenticación.ValidarToken(solicitud.Token)) return new RespuestaValidarNumerico { Exito = false, MensajeError = "Token no autorizado" };

            bool valido = double.TryParse(solicitud.ValorStr, out _);
            return new RespuestaValidarNumerico { Exito = true, EsValido = valido };
        }
    }
}
