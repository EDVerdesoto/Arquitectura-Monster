/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package ec.edu.monster.clienteescritorio.controlador;

import ec.edu.monster.clientesws.*;
import jakarta.xml.ws.BindingProvider;

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
            var portLogin = service.getWSLoginPort();
            ((BindingProvider) portLogin).getRequestContext().put(BindingProvider.ENDPOINT_ADDRESS_PROPERTY, "https://javasoap.dr00p3r.top/WS_ConUni_Java_GR03/WSLogin");
            this.token = portLogin.login(usuario, clave);
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
        var portLon = service.getWSLongitudPort();
        ((BindingProvider) portLon).getRequestContext().put(BindingProvider.ENDPOINT_ADDRESS_PROPERTY, "https://javasoap.dr00p3r.top/WS_ConUni_Java_GR03/WSLongitud");
        return portLon.convertirLongitud(valor, origen, destino, token);
    }

    public double convertirTemperatura(double valor, String origen, String destino) throws Exception {
        WSTemperatura_Service service = new WSTemperatura_Service();
        var portTemp = service.getWSTemperaturaPort();
        ((BindingProvider) portTemp).getRequestContext().put(BindingProvider.ENDPOINT_ADDRESS_PROPERTY, "https://javasoap.dr00p3r.top/WS_ConUni_Java_GR03/WSTemperatura");
        return portTemp.convertirTemperatura(valor, origen, destino, token);
    }

    public double convertirMasa(double valor, String origen, String destino) throws Exception {
        WSMasa_Service service = new WSMasa_Service();
        var portMas = service.getWSMasaPort();
        ((BindingProvider) portMas).getRequestContext().put(BindingProvider.ENDPOINT_ADDRESS_PROPERTY, "https://javasoap.dr00p3r.top/WS_ConUni_Java_GR03/WSMasa");
        return portMas.convertirMasa(valor, origen, destino, token);
    }
}
