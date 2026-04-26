/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package ec.edu.monster.controlador;

import ec.edu.monster.modelo.Conversion;
import ec.edu.monster.servicio.ServicioAuth;
import jakarta.ws.rs.Consumes;
import jakarta.ws.rs.POST;
import jakarta.ws.rs.Path;
import jakarta.ws.rs.Produces;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.Response;
/**
 *
 * @author Soto
 */

@Path("/auth")
@Produces(MediaType.APPLICATION_JSON)
@Consumes(MediaType.APPLICATION_JSON)
public class AuthController {
    private final ServicioAuth authService = new ServicioAuth();

    @POST
    @Path("/login")
    public Response login(Conversion modelo) {

        String token = authService.validarLogin(modelo.getFrom(), modelo.getTo());

        if (token != null) {
            modelo.setToken(token);
            modelo.setSuccess(true);
            modelo.setMessage("Bienvenido al sistema.");
            return Response.ok(modelo).build();
        } else {
            modelo.setSuccess(false);
            modelo.setMessage("Usuario o clave fallidos, intenta de nuevo.");
            return Response.status(Response.Status.UNAUTHORIZED).entity(modelo).build();
        }
    }
}
