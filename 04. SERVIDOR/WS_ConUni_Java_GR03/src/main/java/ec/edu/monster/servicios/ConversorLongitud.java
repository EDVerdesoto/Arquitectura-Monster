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
        String origen = unidadOrigen.toLowerCase().trim();
        String destino = unidadDestino.toLowerCase().trim();

        //Lleva cualquier unidad a metros
        switch (origen) {
            case "centimetros": valorEnMetros = valor / 100.0; break;
            case "metros":      valorEnMetros = valor; break;
            case "pies":        valorEnMetros = valor * 0.3048; break;
            case "yardas":      valorEnMetros = valor * 0.9144; break;
            case "millas":      valorEnMetros = valor * 1609.34; break;
            default: return -1.0; 
        }

        //Lleva de metros a la unidad de destino
        switch (destino) {
            case "centimetros": return valorEnMetros * 100.0;
            case "metros":      return valorEnMetros;
            case "pies":        return valorEnMetros / 0.3048;
            case "yardas":      return valorEnMetros / 0.9144;
            case "millas":      return valorEnMetros / 1609.34;
            default: return -1.0;
        }
    }
}