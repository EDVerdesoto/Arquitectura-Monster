package ec.edu.monster.clienteconsola;

import ec.edu.monster.clientesws.*;

public class ClienteConsola {

    // Extraemos las credenciales a constantes (idealmente vendrían de variables de entorno)
    private static final String WS_USER = "monster";
    private static final String WS_PASS = "monster9";

    public static void main(String[] args) {
        InterfazConsola interfaz = new InterfazConsola();
        OpcionMenu opcion;

        do {
            opcion = interfaz.seleccionarConversion();
            
            if (opcion != null) {
                switch (opcion) {
                    case CONVERTIR_LONGITUD -> ejecutarFlujoConversion(
                            interfaz, 
                            CatalogoUnidades.Longitud.values(), 
                            (v, o, d, u, c) -> new WSLongitud_Service().getWSLongitudPort().convertirLongitud(v, o, d, u, c)
                    );
                    case CONVERTIR_TEMPERATURA -> ejecutarFlujoConversion(
                            interfaz, 
                            CatalogoUnidades.Temperatura.values(), 
                            (v, o, d, u, c) -> new WSTemperatura_Service().getWSTemperaturaPort().convertirTemperatura(v, o, d, u, c)
                    );
                    case CONVERTIR_MASA -> ejecutarFlujoConversion(
                            interfaz, 
                            CatalogoUnidades.Masa.values(), 
                            (v, o, d, u, c) -> new WSMasa_Service().getWSMasaPort().convertirMasa(v, o, d, u, c)
                    );
                    case SALIR -> System.out.println("Saliendo...");
                }
            }
        } while (opcion != OpcionMenu.SALIR);
    }

    /**
     * Interfaz funcional que define la firma de las operaciones SOAP.
     */
    @FunctionalInterface
    private interface OperacionSOAP {
        double ejecutar(double valor, String origen, String destino, String usuario, String clave) throws Exception;
    }

    /**
     * Método genérico que maneja el flujo de consola y la llamada al WS.
     */
    private static <T extends Enum<T>> void ejecutarFlujoConversion(
            InterfazConsola interfaz, 
            T[] valoresCatalogo, 
            OperacionSOAP operacion) {
        
        T origen = interfaz.seleccionarUnidad("Selecciona la unidad de ORIGEN", valoresCatalogo);
        if (origen == null) return;
        
        T destino = interfaz.seleccionarUnidad("Selecciona la unidad de DESTINO", valoresCatalogo);
        if (destino == null) return;

        Double valor = interfaz.pedirValorAConvertir();
        if (valor == null) return;
        
        try {
            double result = operacion.ejecutar(valor, origen.name(), destino.name(), WS_USER, WS_PASS);
            System.out.println("\n" + valor + " " + origen.name().toLowerCase() + " = " + result + " " + destino.name());
        } catch (Exception ex) {
            System.err.println("Error durante la conversión: " + ex.getMessage());
            // TODO: Manejo específico de excepciones SOAP
        }
    }
}