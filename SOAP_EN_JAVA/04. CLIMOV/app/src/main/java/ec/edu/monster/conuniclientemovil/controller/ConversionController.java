package ec.edu.monster.conuniclientemovil.controller;

import ec.edu.monster.conuniclientemovil.model.ConversionRequest;
import ec.edu.monster.conuniclientemovil.model.ConversionResult;
import ec.edu.monster.conuniclientemovil.model.ServiceType;
import ec.edu.monster.conuniclientemovil.model.SoapClient;
import ec.edu.monster.conuniclientemovil.model.SoapErrorMapper;

import java.util.concurrent.ExecutionException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

/**
 * Controlador que coordina las operaciones de login y conversión.
 *
 * Recibe acciones del View, invoca al Model (SoapClient, SoapErrorMapper)
 * y devuelve resultados tipados (LoginResult, ConversionResult)
 * mediante callbacks que el View renderiza.
 */
public class ConversionController {

    /**
     * Callback para operaciones de conversión.
     */
    public interface ConversionCallback {
        void onResult(ConversionResult result);
    }

    private final SoapClient soapClient;
    private final ExecutorService executorService;

    public ConversionController(SoapClient soapClient) {
        this(soapClient, Executors.newSingleThreadExecutor());
    }

    public ConversionController(SoapClient soapClient, ExecutorService executorService) {
        this.soapClient = soapClient;
        this.executorService = executorService;
    }

    /**
     * Ejecuta una conversión de forma asíncrona usando el modelo ConversionRequest.
     */
    public void convert(ConversionRequest request, String baseUrl, ConversionCallback callback) {
        executorService.execute(() -> {
            try {
                double result = executeConversion(
                        request.getServiceType(),
                        baseUrl,
                        request.getValue(),
                        request.getOriginUnit(),
                        request.getDestinationUnit(),
                        request.getToken()
                );

                String mappedError = SoapErrorMapper.mapConversionError(
                        request.getServiceType(), result);

                if (mappedError != null) {
                    callback.onResult(ConversionResult.serviceError(mappedError, result));
                    return;
                }

                callback.onResult(ConversionResult.success(result));
            } catch (Exception exception) {
                callback.onResult(ConversionResult.error(extractMessage(exception)));
            }
        });
    }

    public void shutdown() {
        executorService.shutdown();
    }

    private double executeConversion(
            ServiceType serviceType,
            String baseUrl,
            double inputValue,
            String origin,
            String destination,
            String token
    ) throws Exception {
        switch (serviceType) {
            case LONGITUD:
                return soapClient.convertirLongitud(baseUrl, inputValue, origin, destination, token).get();
            case MASA:
                return soapClient.convertirMasa(baseUrl, inputValue, origin, destination, token).get();
            case TEMPERATURA:
                return soapClient.convertirTemperatura(baseUrl, inputValue, origin, destination, token).get();
            default:
                throw new IllegalArgumentException("Servicio no soportado: " + serviceType);
        }
    }

    private String extractMessage(Exception exception) {
        Throwable cause = exception;
        if (exception instanceof ExecutionException && exception.getCause() != null) {
            cause = exception.getCause();
        }

        String message = cause.getMessage();
        if (message == null || message.trim().isEmpty()) {
            return "Error desconocido";
        }

        return message;
    }
}
