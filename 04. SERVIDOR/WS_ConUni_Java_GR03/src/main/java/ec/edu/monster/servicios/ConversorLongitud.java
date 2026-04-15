/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package ec.edu.monster.servicios;

/**
 *
 * @author Soto
 */
public class ConversorLongitud {

    public double convertirLongitud(double valor, String unidadOrigen, String unidadDestino) {
        double valorEnMetros = 0.0;
        String origen = unidadOrigen.toLowerCase();
        String destino = unidadDestino.toLowerCase();

        //Lleva cualquier unidad a metros
        switch (origen) {
            case "milimetros": valorEnMetros = valor / 1000.0; break;
            case "centimetros": valorEnMetros = valor / 100.0; break;
            case "metros": valorEnMetros = valor; break;
            case "decametros": valorEnMetros = valor * 10.0; break;
            case "kilometros": valorEnMetros = valor * 1000.0; break;
            default: return -1.0; 
        }

        //Lleva de metros a la unidad de destino
        switch (destino) {
            case "milimetros": return valorEnMetros * 1000.0;
            case "centimetros": return valorEnMetros * 100.0;
            case "metros": return valorEnMetros;
            case "decametros": return valorEnMetros / 10.0;
            case "kilometros": return valorEnMetros / 1000.0;
            default: return -1.0;
        }
    }
}