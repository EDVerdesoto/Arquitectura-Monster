/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package ec.edu.monster.controlador;

import ec.edu.monster.modelo.Conversion;
import ec.edu.monster.servicio.ServicioAuth;
import ec.edu.monster.servicio.ConversorMasa;
import ec.edu.monster.servicio.ConversorLongitud;
import ec.edu.monster.servicio.ConversorTemperatura;
import jakarta.ws.rs.Consumes;
import jakarta.ws.rs.GET;
import jakarta.ws.rs.POST;
import jakarta.ws.rs.Path;
import jakarta.ws.rs.Produces;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.Response;

/**
 *
 * @author Soto
 */
@Path("/conversor")
@Produces(MediaType.APPLICATION_JSON)
@Consumes(MediaType.APPLICATION_JSON)
public class ConversorController {

    private final ConversorLongitud longitudService = new ConversorLongitud();
    private final ConversorMasa masaService = new ConversorMasa();
    private final ConversorTemperatura temperaturaService = new ConversorTemperatura();
    private final ServicioAuth authService = new ServicioAuth();

    @POST
    @Path("/longitud")
    public Response convertirLongitud(Conversion modelo) {
        if (!authService.esTokenValido(modelo.getToken())) {
            modelo.setSuccess(false);
            modelo.setMessage("Tu sesión expiró o el token no vale.");
            return Response.status(Response.Status.UNAUTHORIZED).entity(modelo).build();
        }

        try {
            double resultado = longitudService.convertirLongitud(modelo.getValue(), modelo.getFrom(), modelo.getTo());
            modelo.setResult(resultado);
            modelo.setSuccess(true);
            modelo.setMessage("Conversión exitosa.");
            return Response.ok(modelo).build();
        } catch (IllegalArgumentException e) {
            modelo.setSuccess(false);
            modelo.setMessage(e.getMessage());
            return Response.status(Response.Status.BAD_REQUEST).entity(modelo).build();
        }
    }

    @POST
    @Path("/masa")
    public Response convertirMasa(Conversion modelo) {
        if (!authService.esTokenValido(modelo.getToken())) {
            modelo.setSuccess(false);
            modelo.setMessage("Tu sesión expiró o el token no vale.");
            return Response.status(Response.Status.UNAUTHORIZED).entity(modelo).build();
        }

        try {
            double resultado = masaService.convertirMasa(modelo.getValue(), modelo.getFrom(), modelo.getTo());
            modelo.setResult(resultado);
            modelo.setSuccess(true);
            modelo.setMessage("Conversión exitosa.");
            return Response.ok(modelo).build();
        } catch (IllegalArgumentException e) {
            modelo.setSuccess(false);
            modelo.setMessage(e.getMessage());
            return Response.status(Response.Status.BAD_REQUEST).entity(modelo).build();
        }
    }

    @POST
    @Path("/temperatura")
    public Response convertirTemperatura(Conversion modelo) {
        if (!authService.esTokenValido(modelo.getToken())) {
            modelo.setSuccess(false);
            modelo.setMessage("Tu sesión expiró o el token no vale.");
            return Response.status(Response.Status.UNAUTHORIZED).entity(modelo).build();
        }

        try {
            double resultado = temperaturaService.convertirTemperatura(modelo.getValue(), modelo.getFrom(), modelo.getTo());
            modelo.setResult(resultado);
            modelo.setSuccess(true);
            modelo.setMessage("Conversión exitosa.");
            return Response.ok(modelo).build();
        } catch (IllegalArgumentException e) {
            modelo.setSuccess(false);
            modelo.setMessage(e.getMessage());
            return Response.status(Response.Status.BAD_REQUEST).entity(modelo).build();
        }
    }
}
