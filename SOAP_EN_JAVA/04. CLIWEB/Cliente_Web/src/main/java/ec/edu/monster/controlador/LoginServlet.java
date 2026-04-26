/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/JSP_Servlet/Servlet.java to edit this template
 */
package ec.edu.monster.controlador;

import ec.edu.monster.controlador.WSLogin_Service; 
import java.io.IOException;
import java.io.PrintWriter;
import jakarta.servlet.ServletException;
import jakarta.servlet.annotation.WebServlet;
import jakarta.servlet.http.HttpServlet;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import jakarta.servlet.http.HttpSession;

/**
 *
 * @author Soto
 */
@WebServlet(name = "LoginServlet", urlPatterns = {"/LoginServlet"})
public class LoginServlet extends HttpServlet {

    @Override
    protected void doPost(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {

        //Recoger datos del jsp
        String user = request.getParameter("txtUsuario");
        String pass = request.getParameter("txtClave");

        try {
            //Llamar al web service
            WSLogin_Service loginService = new WSLogin_Service();
            var portLogin = loginService.getWSLoginPort();

            String token = portLogin.login(user, pass);

            if (token.contains("ERROR")) {
                request.setAttribute("error", "Credenciales incorrectas, intenta de nuevo.");
                request.getRequestDispatcher("index.jsp").forward(request, response);
            } else {
                HttpSession sesion = request.getSession();
                sesion.setAttribute("miToken", token);
                response.sendRedirect("conversor.jsp");
            }

        } catch (Exception e) {
            request.setAttribute("error", "Error de conexión con el Servidor Payara.");
            request.getRequestDispatcher("index.jsp").forward(request, response);
        }
    }
}
