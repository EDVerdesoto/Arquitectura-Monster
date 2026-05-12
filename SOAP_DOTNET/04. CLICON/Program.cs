using System;
using ClienteConsola.Vista;
using ClienteConsola.Controlador;
using ClienteConsola.Modelo;

namespace ClienteConsola
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InterfazConsola interfaz = new InterfazConsola();
            ControladorConversiones controlador = new ControladorConversiones();

            Console.WriteLine("=== BIENVENIDO AL SISTEMA DE CONVERSIONES ===");
            bool autenticado = false;

            while (!autenticado)
            {
                string usuario = interfaz.pedirTexto("Ingresa tu usuario: ");
                string clave = interfaz.pedirTexto("Ingresa tu clave: ");

                Console.WriteLine("[Autenticando...]");
                autenticado = controlador.iniciarSesion(usuario, clave);

                if (!autenticado)
                {
                    Console.WriteLine("Credenciales incorrectas o error de red. Intenta nuevamente.\n");
                }
            }

            Console.WriteLine("Sesion iniciada con exito\n");

            OpcionMenu? opcion;
            do
            {
                opcion = interfaz.seleccionarConversion();

                if (opcion != null)
                {
                    switch (opcion)
                    {
                        case var _ when opcion == OpcionMenu.CONVERTIR_LONGITUD:
                            ejecutarFlujoConversion(
                                interfaz,
                                Enum.GetValues<CatalogoUnidades.Longitud>(),
                                (v, o, d) => controlador.convertirLongitud(v, o, d)
                            );
                            break;
                        case var _ when opcion == OpcionMenu.CONVERTIR_TEMPERATURA:
                            ejecutarFlujoConversion(
                                interfaz,
                                Enum.GetValues<CatalogoUnidades.Temperatura>(),
                                (v, o, d) => controlador.convertirTemperatura(v, o, d)
                            );
                            break;
                        case var _ when opcion == OpcionMenu.CONVERTIR_MASA:
                            ejecutarFlujoConversion(
                                interfaz,
                                Enum.GetValues<CatalogoUnidades.Masa>(),
                                (v, o, d) => controlador.convertirMasa(v, o, d)
                            );
                            break;
                        case var _ when opcion == OpcionMenu.VALIDAR_NUMERICO:
                            ejecutarValidacionNumerica(interfaz, controlador);
                            break;
                        case var _ when opcion == OpcionMenu.SALIR:
                            Console.WriteLine("Saliendo...");
                            break;
                    }
                }
            } while (opcion != OpcionMenu.SALIR);
        }

        private delegate double OperacionSOAP(double valor, string origen, string destino);

        private static void ejecutarFlujoConversion<T>(
            InterfazConsola interfaz,
            T[] valoresCatalogo,
            OperacionSOAP operacion) where T : struct, Enum
        {
            T? origen = interfaz.seleccionarUnidad("Selecciona la unidad de ORIGEN", valoresCatalogo);
            if (origen == null) return;

            T? destino = interfaz.seleccionarUnidad("Selecciona la unidad de DESTINO", valoresCatalogo);
            if (destino == null) return;

            double? valor = interfaz.pedirValorAConvertir();
            if (valor == null) return;

            Console.WriteLine($"\n[Llamando Controlador...] Convirtiendo {valor} de {origen} a {destino}");

            try
            {
                double result = operacion(valor.Value, origen.Value.ToString(), destino.Value.ToString());
                Console.WriteLine("Resultado = " + result);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error durante la conversion: " + ex.Message);
            }
        }

        private static void ejecutarValidacionNumerica(InterfazConsola interfaz, ControladorConversiones controlador)
        {
            string valor = interfaz.pedirTexto("Ingrese el valor a validar: ");
            try
            {
                bool esValido = controlador.validarCampoNumerico(valor);
                Console.WriteLine($"Es valido: {(esValido ? "Si" : "No")}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error durante la validacion: " + ex.Message);
            }
        }
    }
}
