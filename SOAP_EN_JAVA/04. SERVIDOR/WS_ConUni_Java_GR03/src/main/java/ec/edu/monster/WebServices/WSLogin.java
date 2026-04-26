/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/WebServices/WebService.java to edit this template
 */
package ec.edu.monster.WebServices;

import jakarta.jws.WebService;
import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;
import java.util.UUID;
import java.util.HashMap;
import java.util.Map;

/**
 *
 * @author Soto
 */
@WebService(serviceName = "WSLogin")
public class WSLogin {

    // Guarda los tokens validos 
    public static Map<String, Boolean> tokensValidos = new HashMap<>();

    @WebMethod(operationName = "login")
    public String login(@WebParam(name = "usuario") String usuario, @WebParam(name = "clave") String clave) {
        if ("monster".equals(usuario) && "monster9".equals(clave)) {
            String token = UUID.randomUUID().toString(); //generar token 
            tokensValidos.put(token, true); //guardar token valido 
            return token;
        }
        return "ERROR: Credenciales inválidas";
    }
}
