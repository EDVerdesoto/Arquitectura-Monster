using System.ServiceModel;

namespace ConversionesServerWCF.Modelo
{
    [ServiceContract]
    public interface IServicioConversion
    {
        [OperationContract]
        RespuestaConversion ConvertirLongitud(SolicitudConversion solicitud);

        [OperationContract]
        RespuestaConversion ConvertirTemperatura(SolicitudConversion solicitud);

        [OperationContract]
        RespuestaConversion ConvertirMasa(SolicitudConversion solicitud);

        [OperationContract]
        RespuestaValidarNumerico ValidarCampoNumerico(SolicitudValidarNumerico solicitud);
    }
}
