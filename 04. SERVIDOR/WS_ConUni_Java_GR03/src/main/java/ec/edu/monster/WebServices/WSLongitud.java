/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/WebServices/WebService.java to edit this template
 */
package ec.edu.monster.WebServices;

import ec.edu.monster.servicios.ConversorLongitud; // Importas tu lógica movida
import jakarta.jws.WebService;
import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;

/**
 *
 * @author Soto
 */
@WebService(serviceName = "WSLongitud")
public class WSLongitud {

    ConversorLongitud servicio = new ConversorLongitud();

    @WebMethod(operationName = "convertirLongitud")
    public double convertirLongitud(
            @WebParam(name = "valor") double valor, 
            @WebParam(name = "unidadOrigen") String unidadOrigen, 
            @WebParam(name = "unidadDestino") String unidadDestino,
            @WebParam(name = "token") String token) {
        
        // Validación de seguridad obligatoria
        if (WSLogin.tokensValidos.containsKey(token)) {
            return servicio.convertirLongitud(valor, unidadOrigen, unidadDestino);
        } else {
            return -410.0; 
        }
    }
}