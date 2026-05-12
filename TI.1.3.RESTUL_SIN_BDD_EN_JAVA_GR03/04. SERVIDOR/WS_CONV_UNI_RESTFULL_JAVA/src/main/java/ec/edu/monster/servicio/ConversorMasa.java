/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package ec.edu.monster.servicio;

/**
 *
 * @author Soto
 */
public class ConversorMasa {

    public double convertirMasa(double valor, String unidadOrigen, String unidadDestino) {
        double valorEnKilos = 0.0;
        String origen = unidadOrigen.toLowerCase();
        String destino = unidadDestino.toLowerCase();
        
        //Lleva cualquier unidad a kg
        switch (origen) {
            case "gramos": valorEnKilos = valor / 1000.0; break;
            case "kilogramos": valorEnKilos = valor; break;
            case "onzas": valorEnKilos = valor * 0.0283495; break;
            case "libras": valorEnKilos = valor * 0.453592; break;
            case "quintales": valorEnKilos = (valor * 100.0) * 0.453592; break; 
            default:
                throw new IllegalArgumentException("Unidad de origen no reconocida: " + origen);

        }

        //De kg a la unidad destino
        switch (destino) {
            case "gramos": return valorEnKilos * 1000.0;
            case "kilogramos": return valorEnKilos;
            case "onzas": return valorEnKilos / 0.0283495;
            case "libras": return valorEnKilos / 0.453592;
            case "quintales": return (valorEnKilos / 0.453592) / 100.0;
            default:
                throw new IllegalArgumentException("Unidad de destino no reconocida: " + destino);

        }
    }
}