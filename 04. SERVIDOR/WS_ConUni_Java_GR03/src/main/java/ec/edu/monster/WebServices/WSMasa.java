/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/WebServices/WebService.java to edit this template
 */
package ec.edu.monster.WebServices;

import ec.edu.monster.servicios.ConversorMasa;
import jakarta.jws.WebService;
import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;

/**
 *
 * @author Soto
 */
@WebService(serviceName = "WSMasa")
public class WSMasa {

ConversorMasa servicio = new ConversorMasa();

    @WebMethod(operationName = "convertirMasa")
    public double convertirMasa(
            @WebParam(name = "valor") double valor, 
            @WebParam(name = "unidadOrigen") String unidadOrigen, 
            @WebParam(name = "unidadDestino") String unidadDestino,
            @WebParam(name = "token") String token) {
        
        if (WSLogin.tokensValidos.containsKey(token)) {
            return servicio.convertirMasa(valor, unidadOrigen, unidadDestino);
        } else {
            return -401.0; 
        }
    }
}