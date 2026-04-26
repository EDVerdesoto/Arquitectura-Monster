/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package ec.edu.monster.clienteescritorio.controlador;

import ec.edu.monster.clientesws.*;

/**
 *
 * @author joanc
 */
public class ControladorConversiones {
    private String token;
    
    public boolean iniciarSesion(String usuario, String clave) {
        String msgCredencialesInvalidas = "ERROR: Credenciales inválidas";
        
        try {
            WSLogin_Service service = new WSLogin_Service();
            this.token = service.getWSLoginPort().login(usuario, clave);
            if(this.token.equals(msgCredencialesInvalidas)){
                System.out.println("Credenciales incorrectas");
                return false;
            }
            return true;
        } catch (Exception e) {
            System.err.println("Error en la comunicación con el servicio: " + e.getMessage());
            return false;
        }
    }
    
    public double convertirLongitud(double valor, String origen, String destino) throws Exception {
        WSLongitud_Service service = new WSLongitud_Service();
        return service.getWSLongitudPort().convertirLongitud(valor, origen, destino, token);
    }

    public double convertirTemperatura(double valor, String origen, String destino) throws Exception {
        WSTemperatura_Service service = new WSTemperatura_Service();
        return service.getWSTemperaturaPort().convertirTemperatura(valor, origen, destino, token);
    }

    public double convertirMasa(double valor, String origen, String destino) throws Exception {
        WSMasa_Service service = new WSMasa_Service();
        return service.getWSMasaPort().convertirMasa(valor, origen, destino, token);
    }
}
