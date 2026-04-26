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
    @WebServlet(name = "ConversorServlet", urlPatterns = {"/ConversorServlet"})
    public class ConversorServlet extends HttpServlet {

    private final HttpClient client = HttpClient.newHttpClient();

    @Override
    protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
        response.setContentType("text/plain;charset=UTF-8");
        HttpSession sesion = request.getSession();
        String token = (String) sesion.getAttribute("miToken");

        if (token == null) {
            response.getWriter().write("ERROR_SESSION");
            return;
        }

        String valor = request.getParameter("txtValor");
        String tipo = request.getParameter("cmbTipo");
        String origen = request.getParameter("txtOrigen");
        String destino = request.getParameter("txtDestino");

        // Construimos el JSON para el REST
        String json = String.format("{\"value\":%s, \"from\":\"%s\", \"to\":\"%s\", \"token\":\"%s\"}", 
                                    valor, origen, destino, token);

        HttpRequest restRequest = HttpRequest.newBuilder()
                .uri(URI.create("http://localhost:8080/WS_CONV_UNI_RESTFULL_JAVA/conversor/" + tipo))
                .header("Content-Type", "application/json")
                .POST(HttpRequest.BodyPublishers.ofString(json))
                .build();

        try {
            HttpResponse<String> restResponse = client.send(restRequest, HttpResponse.BodyHandlers.ofString());
            String body = restResponse.body();

            if (restResponse.statusCode() == 200) {
                // Extraemos el resultado del JSON: {"result": 123.45, ...}
                String result = body.split("\"result\":")[1].split(",")[0];
                response.getWriter().write(result);
            } else {
                // Extraemos el mensaje de error del JSON: {"message": "Error tal", ...}
                String errorMsg = body.split("\"message\":\"")[1].split("\"")[0];
                response.getWriter().write("ERROR: " + errorMsg);
            }
        } catch (InterruptedException e) {
            response.getWriter().write("ERROR: Fallo de conexión REST.");
        }
    }
}