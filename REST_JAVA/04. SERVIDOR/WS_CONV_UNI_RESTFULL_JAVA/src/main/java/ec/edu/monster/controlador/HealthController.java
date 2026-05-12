package ec.edu.monster.controlador;

import jakarta.ws.rs.GET;
import jakarta.ws.rs.Path;
import jakarta.ws.rs.Produces;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.Response;

/**
 * Endpoint para verificar si el servidor está vivo.
 */
@Path("/health")
public class HealthController {

    @GET
    @Produces(MediaType.APPLICATION_JSON)
    public Response checkHealth() {
        return Response.ok("{\"status\": \"UP\", \"message\": \"El servidor está vivo\"}").build();
    }
}
