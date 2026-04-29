package ec.edu.monster.conuniclientemovil.controller;

import ec.edu.monster.conuniclientemovil.model.ConversionRequest;
import ec.edu.monster.conuniclientemovil.model.ConversionResult;
import ec.edu.monster.conuniclientemovil.model.ServiceType;
import ec.edu.monster.conuniclientemovil.model.RestClient;

import java.util.concurrent.ExecutionException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

/**
 * Controlador que coordina las operaciones de conversión.
 *
 * Recibe acciones del View, invoca al Model (RestClient)
 * y devuelve resultados tipados (ConversionResult)
 * mediante callbacks que el View renderiza.
 */
public class ConversionController {

    /**
     * Callback para operaciones de conversión.
     */
    public interface ConversionCallback {
        void onResult(ConversionResult result);
    }

    private final RestClient restClient;
    private final ExecutorService executorService;

    public ConversionController(RestClient restClient) {
        this(restClient, Executors.newSingleThreadExecutor());
    }

    public ConversionController(RestClient restClient, ExecutorService executorService) {
        this.restClient = restClient;
        this.executorService = executorService;
    }

    /**
     * Ejecuta una conversión de forma asíncrona usando el modelo ConversionRequest.
     */
    public void convert(ConversionRequest request, String baseUrl, ConversionCallback callback) {
        executorService.execute(() -> {
            try {
                RestClient.ConversionResponse response = executeConversion(
                        request.getServiceType(),
                        baseUrl,
                        request.getValue(),
                        request.getOriginUnit(),
                        request.getDestinationUnit(),
                        request.getToken()
                );

                if (!response.isSuccess()) {
                    // El servidor REST devuelve mensajes de error descriptivos
                    callback.onResult(ConversionResult.serviceError(
                            response.getMessage(), response.getResult()));
                    return;
                }

                callback.onResult(ConversionResult.success(response.getResult()));
            } catch (Exception exception) {
                callback.onResult(ConversionResult.error(extractMessage(exception)));
            }
        });
    }

    public void shutdown() {
        executorService.shutdown();
    }

    private RestClient.ConversionResponse executeConversion(
            ServiceType serviceType,
            String baseUrl,
            double inputValue,
            String origin,
            String destination,
            String token
    ) throws Exception {
        switch (serviceType) {
            case LONGITUD:
                return restClient.convertirLongitud(baseUrl, inputValue, origin, destination, token).get();
            case MASA:
                return restClient.convertirMasa(baseUrl, inputValue, origin, destination, token).get();
            case TEMPERATURA:
                return restClient.convertirTemperatura(baseUrl, inputValue, origin, destination, token).get();
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
