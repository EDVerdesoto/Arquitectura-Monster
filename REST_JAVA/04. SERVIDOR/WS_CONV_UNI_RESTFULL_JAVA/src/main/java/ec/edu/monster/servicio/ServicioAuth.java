/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package ec.edu.monster.servicio;

import java.util.HashMap;
import java.util.Map;
import java.util.UUID;
/**
 *
 * @author Soto
 */
public class ServicioAuth {
    public static final Map<String, Boolean> tokensValidos = new HashMap<>();

    public String validarLogin(String usuario, String clave) {
        if ("monster".equals(usuario) && "monster9".equals(clave)) {
            String token = UUID.randomUUID().toString();
            tokensValidos.put(token, true);
            return token;
        }
        return null;
    }
    
    public boolean esTokenValido(String token) {
        return tokensValidos.containsKey(token);
    }
}
