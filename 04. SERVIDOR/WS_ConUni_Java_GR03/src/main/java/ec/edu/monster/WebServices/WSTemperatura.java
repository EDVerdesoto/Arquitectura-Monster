/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/WebServices/WebService.java to edit this template
 */
package ec.edu.monster.WebServices;

import ec.edu.monster.servicios.ConversorTemperatura;
import jakarta.jws.WebService;
import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;

/**
 *
 * @author Soto
 */
@WebService(serviceName = "WSTemperatura")
public class WSTemperatura {

    ConversorTemperatura servicio = new ConversorTemperatura(); 
    
    @WebMethod(operationName = "convertirTemperatura")
    public double convertirTemperatura(
            @WebParam(name = "valor") double valor, 
            @WebParam(name = "opcionOrigen") String opcionOrigen, 
            @WebParam(name = "opcionDestino") String opcionDestino,
            @WebParam(name = "token") String token) {
        
        if (WSLogin.tokensValidos.containsKey(token)) {
            return servicio.convertirTemperatura(valor, opcionOrigen, opcionDestino);
        } else {
            return -9999999.401; 
        }
    }
}
