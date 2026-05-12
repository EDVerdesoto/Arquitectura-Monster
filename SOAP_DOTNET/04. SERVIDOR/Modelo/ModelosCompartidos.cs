using System.Runtime.Serialization;

namespace ConversionesServerWCF.Modelo
{
    [DataContract]
    public class SolicitudBase
    {
        [DataMember]
        public string Token { get; set; }
    }

    [DataContract]
    public class RespuestaBase
    {
        [DataMember]
        public bool Exito { get; set; }

        [DataMember]
        public string MensajeError { get; set; }
    }

    [DataContract]
    public class SolicitudCredenciales : SolicitudBase
    {
        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Clave { get; set; }
    }

    [DataContract]
    public class RespuestaCredenciales : RespuestaBase
    {
        [DataMember]
        public string TokenGenerado { get; set; }
    }

    [DataContract]
    public class SolicitudConversion : SolicitudBase
    {
        [DataMember]
        public double Valor { get; set; }

        [DataMember]
        public string UnidadOrigen { get; set; }

        [DataMember]
        public string UnidadDestino { get; set; }
    }

    [DataContract]
    public class RespuestaConversion : RespuestaBase
    {
        [DataMember]
        public double ValorConvertido { get; set; }
    }

    [DataContract]
    public class SolicitudValidarNumerico : SolicitudBase
    {
        [DataMember]
        public string ValorStr { get; set; }
    }

    [DataContract]
    public class RespuestaValidarNumerico : RespuestaBase
    {
        [DataMember]
        public bool EsValido { get; set; }
    }
}
