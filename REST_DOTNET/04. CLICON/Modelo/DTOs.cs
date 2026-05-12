namespace ClienteConsola.Modelo
{
    public class LoginRequest
    {
        public string Usuario { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public bool Exito { get; set; }
        public string TokenGenerado { get; set; } = string.Empty;
        public string MensajeError { get; set; } = string.Empty;
    }

    public class RespuestaBase
    {
        public bool Exito { get; set; }
        public string MensajeError { get; set; } = string.Empty;
    }

    public class ConversionRequest
    {
        public double Valor { get; set; }
        public string UnidadOrigen { get; set; } = string.Empty;
        public string UnidadDestino { get; set; } = string.Empty;
    }

    public class ConversionResponse
    {
        public bool Exito { get; set; }
        public double ValorConvertido { get; set; }
        public string MensajeError { get; set; } = string.Empty;
    }

    public class ValidarNumericoRequest
    {
        public string ValorStr { get; set; } = string.Empty;
    }

    public class ValidarNumericoResponse
    {
        public bool Exito { get; set; }
        public bool EsValido { get; set; }
        public string MensajeError { get; set; } = string.Empty;
    }
}