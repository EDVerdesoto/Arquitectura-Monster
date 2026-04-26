/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package ec.edu.monster.servicio;

/**
 *
 * @author Soto
 */
public class ConversorTemperatura {
    public double convertirTemperatura(double valor, String opcionOrigen, String opcionDestino) {
        double valorEnCelsius = 0.0;
        String origen = opcionOrigen.toLowerCase();
        String destino = opcionDestino.toLowerCase();
        
        switch (origen) {
            case "fahrenheit": valorEnCelsius = (valor - 32) * 5.0 / 9.0; break;
            case "kelvin": valorEnCelsius = valor - 273.15; break;
            case "celsius": valorEnCelsius = valor; break;
            case "newton": valorEnCelsius = valor * 100.0 / 33.0; break;
            case "reaumur": valorEnCelsius = valor * 5.0 / 4.0; break;
            default: 
                throw new IllegalArgumentException("Unidad de origen no reconocida: " + origen);
        }

        //Lleva de celsius a la unidad de destino
        switch (destino) {
            case "fahrenheit": return (valorEnCelsius * 9.0 / 5.0) + 32;
            case "kelvin": return valorEnCelsius + 273.15;
            case "celsius": return valorEnCelsius;
            case "newton": return valorEnCelsius * 33.0 / 100.0;
            case "reaumur": return valorEnCelsius * 4.0 / 5.0;
            default: 
                throw new IllegalArgumentException("Unidad de destino no reconocida: " + destino);

        }
       
    }
}