package ec.edu.monster.clienteconsola.vista;

/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */


import java.util.Scanner;

/**
 *
 * @author joanc
 */
public class InterfazConsola {
    private final Scanner entrada;

    public InterfazConsola() {
        this.entrada = new Scanner(System.in);
    }

    public OpcionMenu seleccionarConversion() {
        System.out.println("\n--- Menú de Conversión de Unidades ---");
        
        for (OpcionMenu opcion : OpcionMenu.values()) {
            System.out.println(opcion.getNumero() + ". " + opcion.getDescripcion());
        }
        
        System.out.print("Selecciona una opción: ");
        String seleccion = entrada.nextLine();
        
        for (OpcionMenu opcion : OpcionMenu.values()) {
            if (opcion.getNumero().equals(seleccion)) {
                return opcion;
            }
        }
        
        System.out.println("Opción inválida. Intenta de nuevo.");
        return null;
    }
    
    public <T extends Enum<T>> T seleccionarUnidad(String titulo, T[] opciones) {
        System.out.println("\n--- " + titulo + " ---");
        for (int i = 0; i < opciones.length; i++) {
            System.out.println((i + 1) + ". " + opciones[i].name());
        }
        System.out.print("Elige una unidad: ");
        
        try {
            int seleccion = Integer.parseInt(entrada.nextLine());
            if (seleccion >= 1 && seleccion <= opciones.length) {
                return opciones[seleccion - 1];
            }
        } catch (NumberFormatException e) {
            // Ignorar para que caiga en el error de abajo
        }
        
        System.out.println("Unidad inválida. Operación cancelada.");
        return null;
    }
    
    public Double pedirValorAConvertir() {
        System.out.print("\nIngresa el valor a convertir: ");
        try {
            return Double.valueOf(entrada.nextLine());
        } catch (NumberFormatException e) {
            System.out.println("Debe ser un número válido.");
            return null;
        }
    }
    
    public String pedirTexto(String mensaje) {
        System.out.print(mensaje);
        return entrada.nextLine().trim();
    }
}
