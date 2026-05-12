package ec.edu.monster.conuniclientemovil.controller;

import ec.edu.monster.conuniclientemovil.model.LoginResult;
import ec.edu.monster.conuniclientemovil.model.SoapClient;

import java.util.concurrent.ExecutionException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class LoginController {

    /**
     * Callback para el resultado del login.
     */
    public interface LoginCallback {
        void onResult(LoginResult result);
    }

    private final SoapClient soapClient;
    private final ExecutorService executorService;

    public LoginController(SoapClient soapClient) {
        this(soapClient, Executors.newSingleThreadExecutor());
    }

    public LoginController(SoapClient soapClient, ExecutorService executorService) {
        this.soapClient = soapClient;
        this.executorService = executorService;
    }

    /**
     * Ejecuta el login de forma asíncrona.
     */
    public void login(String baseUrl, String usuario, String clave, LoginCallback callback) {
        executorService.execute(() -> {
            try {
                String token = soapClient.login(baseUrl, usuario, clave).get();
                callback.onResult(LoginResult.success(token));
            } catch (Exception exception) {
                callback.onResult(LoginResult.error(extractMessage(exception)));
            }
        });
    }

    public void shutdown() {
        executorService.shutdown();
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
