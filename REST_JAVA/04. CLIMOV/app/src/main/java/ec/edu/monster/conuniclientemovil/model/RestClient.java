package ec.edu.monster.conuniclientemovil.model;

import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.nio.charset.StandardCharsets;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;

/**
 * Cliente REST que ejecuta llamadas al servidor RESTful de ConUni.
 *
 * Forma parte de la capa Model: es la fuente de datos remota.
 * No contiene lógica de presentación ni de coordinación de UI.
 *
 * Reemplaza al antiguo SoapClient, comunicándose ahora mediante
 * peticiones HTTP POST con cuerpo JSON.
 */
public class RestClient {

    private static final int TIMEOUT_MS = 15000;

    private final ExecutorService executorService;
    private volatile String sessionToken;

    public RestClient() {
        this(Executors.newFixedThreadPool(2));
    }

    public RestClient(ExecutorService executorService) {
        this.executorService = executorService;
    }

    /**
     * Realiza login contra el servidor REST.
     *
     * @param baseUrl  URL base del servidor (ej. http://10.0.2.2:8080/WS_CONV_UNI_RESTFULL_JAVA-1.0-SNAPSHOT/)
     * @param usuario  nombre de usuario
     * @param clave    contraseña
     * @return Future con el token de sesión
     */
    public Future<String> login(String baseUrl, String usuario, String clave) {
        return executorService.submit(() -> {
            JSONObject body = new JSONObject();
            body.put("from", usuario);
            body.put("to", clave);

            JSONObject response = doPost(buildEndpoint(baseUrl, "auth/login"), body);

            boolean success = response.optBoolean("success", false);
            if (!success) {
                String message = response.optString("message", "Login fallido.");
                throw new IOException(message);
            }

            String token = response.optString("token", null);
            if (token == null || token.isEmpty() || "null".equals(token)) {
                throw new IOException("El servidor no devolvió un token válido.");
            }

            sessionToken = token;
            return token;
        });
    }

    /**
     * Convierte unidades de longitud.
     */
    public Future<ConversionResponse> convertirLongitud(
            String baseUrl, double valor, String unidadOrigen,
            String unidadDestino, String token) {
        return executorService.submit(() ->
                doConversion(baseUrl, "conversor/longitud", valor, unidadOrigen, unidadDestino, token));
    }

    /**
     * Convierte unidades de masa.
     */
    public Future<ConversionResponse> convertirMasa(
            String baseUrl, double valor, String unidadOrigen,
            String unidadDestino, String token) {
        return executorService.submit(() ->
                doConversion(baseUrl, "conversor/masa", valor, unidadOrigen, unidadDestino, token));
    }

    /**
     * Convierte unidades de temperatura.
     */
    public Future<ConversionResponse> convertirTemperatura(
            String baseUrl, double valor, String opcionOrigen,
            String opcionDestino, String token) {
        return executorService.submit(() ->
                doConversion(baseUrl, "conversor/temperatura", valor, opcionOrigen, opcionDestino, token));
    }

    public String getSessionToken() {
        return sessionToken;
    }

    public void clearSessionToken() {
        sessionToken = null;
    }

    public void shutdown() {
        executorService.shutdown();
    }

    /* ══════════════════════════════════════════════════════
       Métodos internos
       ══════════════════════════════════════════════════════ */

    private ConversionResponse doConversion(String baseUrl, String path,
                                            double valor, String from, String to, String token) throws Exception {
        JSONObject body = new JSONObject();
        body.put("from", from);
        body.put("to", to);
        body.put("value", valor);
        body.put("token", token);

        JSONObject response = doPost(buildEndpoint(baseUrl, path), body);

        boolean success = response.optBoolean("success", false);
        double result = response.optDouble("result", 0.0);
        String message = response.optString("message", "");

        return new ConversionResponse(success, result, message);
    }

    private JSONObject doPost(String endpoint, JSONObject body) throws Exception {
        URL url = new URL(endpoint);
        HttpURLConnection connection = (HttpURLConnection) url.openConnection();

        try {
            connection.setRequestMethod("POST");
            connection.setRequestProperty("Content-Type", "application/json; charset=UTF-8");
            connection.setRequestProperty("Accept", "application/json");
            connection.setDoOutput(true);
            connection.setConnectTimeout(TIMEOUT_MS);
            connection.setReadTimeout(TIMEOUT_MS);

            // Enviar el cuerpo JSON
            byte[] outputBytes = body.toString().getBytes(StandardCharsets.UTF_8);
            try (OutputStream os = connection.getOutputStream()) {
                os.write(outputBytes);
                os.flush();
            }

            // Leer la respuesta
            int statusCode = connection.getResponseCode();
            BufferedReader reader;

            if (statusCode >= 200 && statusCode < 300) {
                reader = new BufferedReader(
                        new InputStreamReader(connection.getInputStream(), StandardCharsets.UTF_8));
            } else {
                // Para errores 4xx/5xx, leer el errorStream
                if (connection.getErrorStream() != null) {
                    reader = new BufferedReader(
                            new InputStreamReader(connection.getErrorStream(), StandardCharsets.UTF_8));
                } else {
                    throw new IOException("Error HTTP " + statusCode + " sin cuerpo de respuesta.");
                }
            }

            StringBuilder responseBuilder = new StringBuilder();
            String line;
            while ((line = reader.readLine()) != null) {
                responseBuilder.append(line);
            }
            reader.close();

            String responseString = responseBuilder.toString();
            if (responseString.isEmpty()) {
                throw new IOException("Respuesta vacía del servidor (HTTP " + statusCode + ").");
            }

            return new JSONObject(responseString);
        } finally {
            connection.disconnect();
        }
    }

    private String buildEndpoint(String baseUrl, String path) {
        if (baseUrl == null || baseUrl.trim().isEmpty()) {
            throw new IllegalArgumentException("baseUrl no puede estar vacía");
        }

        String normalizedBaseUrl = baseUrl.trim();
        if (!normalizedBaseUrl.endsWith("/")) {
            normalizedBaseUrl = normalizedBaseUrl + "/";
        }

        return normalizedBaseUrl + path;
    }

    /* ══════════════════════════════════════════════════════
       Clase interna para la respuesta de conversión
       ══════════════════════════════════════════════════════ */

    /**
     * Encapsula la respuesta JSON del servidor para una conversión.
     */
    public static class ConversionResponse {
        private final boolean success;
        private final double result;
        private final String message;

        public ConversionResponse(boolean success, double result, String message) {
            this.success = success;
            this.result = result;
            this.message = message;
        }

        public boolean isSuccess() {
            return success;
        }

        public double getResult() {
            return result;
        }

        public String getMessage() {
            return message;
        }
    }
}
