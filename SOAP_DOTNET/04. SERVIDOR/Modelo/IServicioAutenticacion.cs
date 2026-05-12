using System.ServiceModel;

namespace ConversionesServerWCF.Modelo
{
    [ServiceContract]
    public interface IServicioAutenticacion
    {
        [OperationContract]
        RespuestaCredenciales ValidarCredenciales(SolicitudCredenciales solicitud);


    }
}
