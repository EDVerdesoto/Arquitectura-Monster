package ec.edu.monster.clienteescritorio.controlador;

import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.time.Duration;

/**
 *
 * @author joanc
 */
public class ControladorConversiones {
    private String token;
    // URL directa sin el /api/
    private static final String BASE_URL = "https://javarest.dr00p3r.top/WS_CONV_UNI_RESTFULL_JAVA";
    private final HttpClient client = HttpClient.newBuilder()
            .connectTimeout(Duration.ofSeconds(5))
            .build();

    public boolean iniciarSesion(String usuario, String clave) {
        // Armamos el JSON manual para no meter librerias extra (Java Purito)
        String json = String.format("{\"from\":\"%s\", \"to\":\"%s\"}", usuario, clave);

        try {
            HttpRequest request = HttpRequest.newBuilder()
                    .uri(URI.create(BASE_URL + "/auth/login"))
                    .header("Content-Type", "application/json")
                    .POST(HttpRequest.BodyPublishers.ofString(json))
                    .build();

            HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());

            if (response.statusCode() == 200) {
                // Parseo manual rapido para sacar el token del JSON
                this.token = response.body().split("\"token\":\"")[1].split("\"")[0];
                return true;
            } else {
                return false;
            }
        } catch (Exception e) {
            System.err.println("Error en comunicacion REST: " + e.getMessage());
            return false;
        }
    }

    // Metodo privado para reutilizar la logica de peticiones POST
    private double ejecutarPeticion(String path, double valor, String origen, String destino) throws Exception {
        String json = String.format("{\"value\":%s, \"from\":\"%s\", \"to\":\"%s\", \"token\":\"%s\"}",
                valor, origen, destino, this.token);

        HttpRequest request = HttpRequest.newBuilder()
                .uri(URI.create(BASE_URL + "/conversor/" + path))
                .header("Content-Type", "application/json")
                .POST(HttpRequest.BodyPublishers.ofString(json))
                .build();

        HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());
        String body = response.body();

        if (response.statusCode() == 200) {
            // Extraemos el valor del campo "result"
            String res = body.split("\"result\":")[1].split(",")[0];
            return Double.parseDouble(res);
        } else {
            // Si el servidor mando un error (ej. unidades invalidas), sacamos el mensaje
            String errorMsg = body.split("\"message\":\"")[1].split("\"")[0];
            throw new Exception(errorMsg);
        }
    }

    public double convertirLongitud(double valor, String origen, String destino) throws Exception {
        return ejecutarPeticion("longitud", valor, origen, destino);
    }

    public double convertirTemperatura(double valor, String origen, String destino) throws Exception {
        return ejecutarPeticion("temperatura", valor, origen, destino);
    }

    public double convertirMasa(double valor, String origen, String destino) throws Exception {
        return ejecutarPeticion("masa", valor, origen, destino);
    }
}