/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package ec.edu.monster.vista;

import ec.edu.monster.controlador.WSLogin_Service;
import ec.edu.monster.controlador.WSLongitud_Service;
import ec.edu.monster.controlador.WSTemperatura_Service;
import ec.edu.monster.controlador.WSMasa_Service;
import java.util.Scanner;
/**
 *
 * @author Soto
 */
public class Main {

    public static void main(String[] args) {
        Scanner sc = new Scanner(System.in);
        String token = "";

        System.out.println("=== SISTEMA DE CONVERSIONES ESPE ===");
        System.out.println("Inicie sesión para continuar");
        System.out.print("Usuario: ");
        String user = sc.next();
        System.out.print("Contraseña: ");
        String pass = sc.next();

        try {
            WSLogin_Service loginService = new WSLogin_Service();
            var portLogin = loginService.getWSLoginPort();
            token = portLogin.login(user, pass);

            if (token.contains("ERROR")) {
                System.out.println("\n[X] Credenciales incorrectas. Apagando sistema.");
                return;
            }
            System.out.println("\n[OK] Ingreso exitoso. Token asignado.");
        } catch (Exception e) {
            System.out.println("\n[X] Error conectando al servidor. ¿Está prendido el Payara?");
            return;
        }

        int opcion = 0;
        do {
            System.out.println("\n--- MENÚ PRINCIPAL ---");
            System.out.println("1. Longitud");
            System.out.println("2. Masa");
            System.out.println("3. Temperatura");
            System.out.println("4. Salir");
            System.out.print("Elija una opción: ");

            try {
                opcion = Integer.parseInt(sc.next());
            } catch (NumberFormatException e) {
                System.out.println("¡Oye! Solo pon números del 1 al 4.");
                continue;
            }

            if (opcion >= 1 && opcion <= 3) {
                System.out.print("Ingrese el valor a convertir: ");
                double valor = 0;
                try {
                    valor = Double.parseDouble(sc.next());
                } catch (NumberFormatException e) {
                    System.out.println("¡Valor inválido! No pongas letras.");
                    continue;
                }

                double resultado = 0;

                switch (opcion) {
                    case 1:
                        System.out.print("Unidad Origen (metros, pies, yardas...): ");
                        String oLon = sc.next();
                        System.out.print("Unidad Destino: ");
                        String dLon = sc.next();

                        var portLongitud = new WSLongitud_Service().getWSLongitudPort();
                        resultado = portLongitud.convertirLongitud(valor, oLon, dLon, token);
                        break;

                    case 2:
                        System.out.print("Unidad Origen (gramos, libras, quintales...): ");
                        String oMas = sc.next();
                        System.out.print("Unidad Destino: ");
                        String dMas = sc.next();

                        var portMasa = new WSMasa_Service().getWSMasaPort();
                        resultado = portMasa.convertirMasa(valor, oMas, dMas, token);
                        break;

                    case 3:
                        System.out.println("Unidades: fahrenheit | kelvin | celsius | newton | reaumur");
                        System.out.print("Unidad Origen: ");
                        String oTemp = sc.next();
                        System.out.print("Unidad Destino: ");
                        String dTemp = sc.next();

                        var portTemp = new WSTemperatura_Service().getWSTemperaturaPort();
                        resultado = portTemp.convertirTemperatura(valor, oTemp, dTemp, token);
                        break;
                }

                if (resultado == -401.0) {
                    System.out.println("\n[X] Error de Seguridad: Token inválido.");
                } else if (resultado == -1.0 || resultado == -9999.99) {
                    System.out.println("\n[X] Error: Unidad de medida no reconocida.");
                } else if (resultado == -2.0) {
                    System.out.println("\n[X] Error: El valor ingresado no es válido para esta magnitud.");
                } else {
                    System.out.println("\n>>> RESULTADO DE LA CONVERSIÓN: " + resultado);
                }
            }
        } while (opcion != 4);

        System.out.println("Saliendo del sistema...");
        sc.close();
    }
}