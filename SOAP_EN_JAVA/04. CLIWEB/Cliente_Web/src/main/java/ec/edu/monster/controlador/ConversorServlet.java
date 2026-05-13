    /*
     * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
     * Click nbfs://nbhost/SystemFileSystem/Templates/JSP_Servlet/Servlet.java to edit this template
     */
    package ec.edu.monster.controlador;

    import java.io.IOException;
    import jakarta.servlet.ServletException;
    import jakarta.servlet.annotation.WebServlet;
    import jakarta.servlet.http.HttpServlet;
    import jakarta.servlet.http.HttpServletRequest;
    import jakarta.servlet.http.HttpServletResponse;
    import jakarta.servlet.http.HttpSession;
    import ec.edu.monster.clientesws.*;
    
    import jakarta.xml.ws.BindingProvider;
    /**
     *
     * @author Soto
     */
    @WebServlet(name = "ConversorServlet", urlPatterns = {"/ConversorServlet"})
    public class ConversorServlet extends HttpServlet {

        @Override
        protected void doPost(HttpServletRequest request, HttpServletResponse response)
                throws ServletException, IOException {

            response.setContentType("text/plain;charset=UTF-8");

            HttpSession sesion = request.getSession();
            String token = (String) sesion.getAttribute("miToken");

            if (token == null) {
                response.getWriter().write("ERROR_SESSION");
                return;
            }

            double valor = Double.parseDouble(request.getParameter("txtValor"));
            String tipo = request.getParameter("cmbTipo");
            String origen = request.getParameter("txtOrigen");
            String destino = request.getParameter("txtDestino");

            if (valor < 0 && (tipo.equals("longitud") || tipo.equals("masa"))) {
                response.getWriter().write("ERROR: No existen valores negativos en " + tipo + ".");
                return;
            }

            double resultado = 0;

            try {
                switch (tipo) {
                    case "longitud":
                        if (valor < 0){
                            response.getWriter().write("ERROR: No se permiten valores negativos para esta magnitud.");
                            break;
                        }
                        var portLon = new WSLongitud_Service().getWSLongitudPort();
                        ((BindingProvider) portLon).getRequestContext().put(BindingProvider.ENDPOINT_ADDRESS_PROPERTY, "https://javasoap.dr00p3r.top/WS_ConUni_Java_GR03/WSLongitud");
                        resultado = portLon.convertirLongitud(valor, origen, destino, token);
                        break;
                    case "masa":
                        if (valor < 0){
                            response.getWriter().write("ERROR: No se permiten valores negativos para esta magnitud.");
                            break;
                        }
                        var portMas = new WSMasa_Service().getWSMasaPort();
                        ((BindingProvider) portMas).getRequestContext().put(BindingProvider.ENDPOINT_ADDRESS_PROPERTY, "https://javasoap.dr00p3r.top/WS_ConUni_Java_GR03/WSMasa");
                        resultado = portMas.convertirMasa(valor, origen, destino, token);
                        break;
                    case "temperatura":
                        var portTemp = new WSTemperatura_Service().getWSTemperaturaPort();
                        ((BindingProvider) portTemp).getRequestContext().put(BindingProvider.ENDPOINT_ADDRESS_PROPERTY, "https://javasoap.dr00p3r.top/WS_ConUni_Java_GR03/WSTemperatura");
                        resultado = portTemp.convertirTemperatura(valor, origen, destino, token);
                        break;
                }

                if (resultado == -9999999.401) {
                    response.getWriter().write("ERROR: Sesión inválida. Loguéate de nuevo.");
                } else if (resultado == -999999.404) {
                    response.getWriter().write("ERROR: Unidades no reconocidas por el sistema.");
                } else {
                    response.getWriter().write(String.valueOf(resultado));
                }

            } catch (Exception e) {
                response.getWriter().write("ERROR: Fallo al conectar con el servicio web.");
            }
        }
}
