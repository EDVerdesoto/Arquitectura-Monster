/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/JSP_Servlet/Servlet.java to edit this template
 */
package ec.edu.monster.controlador;

import jakarta.servlet.ServletException;
import jakarta.servlet.annotation.WebServlet;
import jakarta.servlet.http.*;
import java.io.IOException;
import java.net.URI;
import java.net.http.*;

/**
 *
 * @author Soto
 */
@WebServlet(name = "LoginServlet", urlPatterns = {"/LoginServlet"})
public class LoginServlet extends HttpServlet {

    // Herramienta nativa de Java para consumir REST
    private final HttpClient client = HttpClient.newHttpClient();

    @Override
    protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
        String user = request.getParameter("txtUsuario");
        String pass = request.getParameter("txtClave");

        // 1. Armamos el JSON tal como lo espera nuestro modelo único de REST
        String json = String.format("{\"from\":\"%s\", \"to\":\"%s\"}", user, pass);

        // 2. Apuntamos a la URL correcta de tu nuevo servidor Payara
        HttpRequest restRequest = HttpRequest.newBuilder()
                .uri(URI.create("http://localhost:8081/WS_CONV_UNI_RESTFULL_JAVA/auth/login"))
                .header("Content-Type", "application/json")
                .POST(HttpRequest.BodyPublishers.ofString(json))
                .build();

        try {
            // 3. Enviamos la petición y esperamos la respuesta
            HttpResponse<String> restResponse = client.send(restRequest, HttpResponse.BodyHandlers.ofString());
            
            // 4. Leemos el resultado
            if (restResponse.statusCode() == 200) {
                String body = restResponse.body();
                // Extraemos el token del texto JSON
                String token = body.split("\"token\":\"")[1].split("\"")[0];
                
                request.getSession().setAttribute("miToken", token);
                response.sendRedirect("conversor.jsp");
            } else {
                request.setAttribute("error", "Credenciales incorrectas.");
                request.getRequestDispatcher("index.jsp").forward(request, response);
            }
        } catch (Exception e) {
            request.setAttribute("error", "Error de conexión con el servidor REST.");
            request.getRequestDispatcher("index.jsp").forward(request, response);
        }
    }
}