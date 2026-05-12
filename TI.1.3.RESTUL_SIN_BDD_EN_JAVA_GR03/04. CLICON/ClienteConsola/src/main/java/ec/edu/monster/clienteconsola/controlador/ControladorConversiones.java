/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package ec.edu.monster.clienteconsola.controlador;

import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;

/**
 *
 * @author joanc
 */
public class ControladorConversiones {
    private String token;
    private static final String BASE_URL = "http://localhost:8081/WS_CONV_UNI_RESTFULL_JAVA";
    private final HttpClient client = HttpClient.newHttpClient();

    public boolean iniciarSesion(String usuario, String clave) {
        // Armamos el JSON reciclando el modelo para mandar credenciales
        String json = String.format("{\"from\":\"%s\", \"to\":\"%s\"}", usuario, clave);

        try {
            HttpRequest request = HttpRequest.newBuilder()
                    .uri(URI.create(BASE_URL + "/auth/login"))
                    .header("Content-Type", "application/json")
                    .POST(HttpRequest.BodyPublishers.ofString(json))
                    .build();

            HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());

            if (response.statusCode() == 200) {
                // Parseo manual rápido para sacar el token
                this.token = response.body().split("\"token\":\"")[1].split("\"")[0];
                return true;
            } else {
                System.out.println("Credenciales incorrectas");
                return false;
            }
        } catch (Exception e) {
            System.err.println("Error en la comunicación con el servicio REST: " + e.getMessage());
            return false;
        }
    }

    // Método genérico para no repetir el código del HttpClient en cada conversión
    private double hacerPeticionRest(String endpoint, double valor, String origen, String destino) throws Exception {
        String json = String.format("{\"value\":%s, \"from\":\"%s\", \"to\":\"%s\", \"token\":\"%s\"}",
                valor, origen, destino, this.token);

        HttpRequest request = HttpRequest.newBuilder()
                .uri(URI.create(BASE_URL + "/conversor/" + endpoint))
                .header("Content-Type", "application/json")
                .POST(HttpRequest.BodyPublishers.ofString(json))
                .build();

        HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());
        String body = response.body();

        if (response.statusCode() == 200) {
            // Extraer el número del JSON ("result": 123.4)
            String resultado = body.split("\"result\":")[1].split(",")[0];
            return Double.parseDouble(resultado);
        } else {
            // Extraer el texto de error del JSON y lanzarlo para que ClienteConsola lo atrape
            String error = body.split("\"message\":\"")[1].split("\"")[0];
            throw new Exception(error);
        }
    }

    public double convertirLongitud(double valor, String origen, String destino) throws Exception {
        return hacerPeticionRest("longitud", valor, origen, destino);
    }

    public double convertirTemperatura(double valor, String origen, String destino) throws Exception {
        return hacerPeticionRest("temperatura", valor, origen, destino);
    }

    public double convertirMasa(double valor, String origen, String destino) throws Exception {
        return hacerPeticionRest("masa", valor, origen, destino);
    }
}